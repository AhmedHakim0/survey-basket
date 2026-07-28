
namespace SurveyBasket.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync([FromBody] AuthRequest request,CancellationToken cancellationToken)
    {
        var AuthResult = await _authService.GenerateToken(request.Email, request.Password, cancellationToken);

        return AuthResult is null ? BadRequest("Invalid email or password") : Ok(AuthResult);
    }

   
}
