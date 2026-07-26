using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Services;

public class AuthService(UserManager<ApplicationUser> userManager) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;

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

        //generating token

        return new AuthResponse
        (
            user.Id,
            user.Email,
            user.FirstName,
            user.LastName,
            "token", // Replace with actual token generation
            3600 // Replace with actual expiration time
        );
    }
}
