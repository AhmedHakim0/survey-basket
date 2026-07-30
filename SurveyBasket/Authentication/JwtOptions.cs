
namespace SurveyBasket.Authentication;

public class JwtOptions
{
    public static readonly string SectionName = "jwt";

    [Required]
    public string Key { get; set; } = string.Empty;

    [Required]
    public string issuer { get; set; } = string.Empty;

    [Required]
    public string audience { get; set; } = string.Empty;

    [Range(1,int.MaxValue)]
    public int ExpiryMinutes { get; set; }
}
