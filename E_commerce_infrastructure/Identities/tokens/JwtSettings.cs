namespace E_commerce_infrastructure.Identities;

public class JwtSettinngs
{
    public const string SectionName = "JWT";

    public string Issuer { get; set; } = null!;

    public string Audience { get; set; } = null!;
    
    public string SecretKey { get; set; } = null!;
    
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
    
}