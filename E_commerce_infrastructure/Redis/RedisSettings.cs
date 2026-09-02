namespace E_commerce_infrastructure.Redis;

public class RedisSettings
{
    public const string SectionName = "RedisSettings";
    
    public string Endpoint { get; set; } = null!;
    public int Port { get; set; }
    public string User { get; set; } = null!;
    public string Password { get; set; } = null!;
}