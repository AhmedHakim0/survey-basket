
namespace SurveyBasket.Authentication;

public class JwtProvidor(IConfiguration configuration) : IJwtProvidor
{
    private readonly IConfiguration _configuration = configuration;

    
    public (string token, int expiresIn) GenerateToken(ApplicationUser user)
    {
        Claim[] claims = new Claim[] {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName!),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"]!)); 
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var ExpiresIn = 30; 

        var expirationTime = DateTime.UtcNow.AddMinutes(ExpiresIn);

        var token= new JwtSecurityToken(
            issuer: _configuration["jwt:issuer"],
            audience: _configuration["jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["jwt:ExpireMinutes"])),
            signingCredentials: signingCredentials
        );

        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: Convert.ToInt32(_configuration["jwt:ExpireMinutes"]));
    }
}
