
namespace SurveyBasket.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvidor jwtProvidor) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvidor _jwtProvidor = jwtProvidor;
    private readonly int _refreshTokenExpirationDays = 14;
    public async Task<Result<AuthResponse>> GenerateToken(string email, string password, CancellationToken cancellationToken)
    {
        /*
        cheking for user Existence
        */
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        /*
         * checking for password validity
         */
        var IsValidPassword = await _userManager.CheckPasswordAsync(user, password);

        if (!IsValidPassword)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        /*
         * Generating JWT token and refresh token
         */
        var (token, expiresIn) = _jwtProvidor.GenerateToken(user);

        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        var response = new AuthResponse
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
        return Result.Success(response);
    }

    public async Task<Result<AuthResponse>> GenerateRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken)
    {
        /*
         * Validating the token and getting the userId from it
         */
        var userId = _jwtProvidor.ValidateToken(token);
        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        /*
         * Getting the user from the database
         */
        var user = _userManager.Users.FirstOrDefault(u => u.Id == userId);
        if(user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);

        /*
         * Checking if the refresh token is valid and active
         */
        var existingRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (existingRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidToken);
        
        /*
         * Revoking the existing refresh token
         */
         existingRefreshToken.RevokedOn = DateTime.UtcNow;
        var (newtoken, expiresIn) = _jwtProvidor.GenerateToken(user);

        /*
         * Generating a new refresh token and adding it to the user's refresh tokens
         */
        var newrefreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpirationDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newrefreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        var result = new AuthResponse
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

        return Result.Success(result);

    }

    /*
     * Generating a random refresh token
     */
    private static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    
}
