
namespace SurveyBasket.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvidor jwtProvidor) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvidor _jwtProvidor = jwtProvidor;

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

        return new AuthResponse
        (
            user.Id,
            user.Email,
            user.FirstName!,
            user.LastName!,
            token,
            expiresIn
        );
    }

  
}
