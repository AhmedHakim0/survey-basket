
namespace SurveyBasket.Authentication;

public class JwtProvidor : IJwtProvidor
{
    public (string token, int expiresIn) GenerateToken(ApplicationUser user)
    {
        Claim[] claims = new Claim[] {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName!),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("u3pUxTfOnt3BinjY5u2Xdt0wqzNY1NDw")); 
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var ExpiresIn = 30; 

        var expirationTime = DateTime.UtcNow.AddMinutes(ExpiresIn);

        var token= new JwtSecurityToken(
            issuer: "SurveyBasket",
            audience: "SurveyBasketApp users",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(ExpiresIn),
            signingCredentials: signingCredentials
        );

        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: ExpiresIn);
    }
}
