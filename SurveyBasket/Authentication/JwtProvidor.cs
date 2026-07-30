
using Microsoft.Extensions.Options;

namespace SurveyBasket.Authentication;

public class JwtProvidor(IOptionsSnapshot<JwtOptions> jwtOptions) : IJwtProvidor
{
    private readonly IOptionsSnapshot<JwtOptions> _jwtOptions = jwtOptions;

    public (string token, int expiresIn) GenerateToken(ApplicationUser user)
    {
        Claim[] claims = new Claim[] {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.GivenName, user.FirstName!),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.Key)); 
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        

        var expirationTime = DateTime.UtcNow.AddMinutes(_jwtOptions.Value.ExpiryMinutes);

        var token= new JwtSecurityToken(
            issuer: _jwtOptions.Value.issuer,
            audience: _jwtOptions.Value.audience,
            claims: claims,
            expires: expirationTime,
            signingCredentials: signingCredentials
        );

        return (token: new JwtSecurityTokenHandler().WriteToken(token), expiresIn: Convert.ToInt32(_jwtOptions.Value.ExpiryMinutes));
    }
}
