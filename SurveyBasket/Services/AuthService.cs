
namespace SurveyBasket.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvidor jwtProvidor) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvidor _jwtProvidor = jwtProvidor;
    private readonly int _refreshTokenExpirationDays = 14;
    public async Task<AuthResponse?> GenerateToken(string email, string password, CancellationToken cancellationToken)
    {
        //cheking for user
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return null;

        //checking for password
        var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!IsValidPassword)
            return null;

        var (token, expiresIn) = _jwtProvidor.GenerateToken(user);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        return new AuthResponse
        (
            user.Id,
            user.Email,
            user.FirstName!,
            user.LastName!,
            token,
            expiresIn,
            refreshToken,
            refreshTokenExpiration
        );
    }

    public async Task<AuthResponse?> GenerateRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
    {
        var userId = _jwtProvidor.ValidateToken(token);
        if (userId is null)
            return null;

        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
        if(user is null)
            return null;

        var existingRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (existingRefreshToken is null)
            return null;

         existingRefreshToken.RevokedOn = DateTime.UtcNow;
        var (newtoken, expiresIn) = _jwtProvidor.GenerateToken(user);

        var newrefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newrefreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        return new AuthResponse
        (
            user.Id,
            user.Email,
            user.FirstName!,
            user.LastName!,
            newtoken,
            expiresIn,
            newrefreshToken,
            refreshTokenExpiration
        );

    }


    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    
}
