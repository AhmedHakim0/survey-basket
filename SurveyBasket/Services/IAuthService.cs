namespace SurveyBasket.Services;

public interface IAuthService
{
    Task<AuthResponse?> GenerateToken(string email, string password, CancellationToken cancellationToken);
    Task<AuthResponse?> GenerateRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
}
