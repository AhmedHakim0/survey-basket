namespace SurveyBasket.Services;

public interface IAuthService
{
    Task<Result<AuthResponse>> GenerateToken(string email, string password, CancellationToken cancellationToken);
    Task<Result<AuthResponse>> GenerateRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
}
