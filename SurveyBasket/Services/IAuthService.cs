namespace SurveyBasket.Services;

public interface IAuthService
{
    Task<AuthResponse?> GenerateToken(string email, string password, CancellationToken cancellationToken); 
}
