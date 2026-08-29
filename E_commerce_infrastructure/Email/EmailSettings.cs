namespace E_commerce_infrastructure.Email;

public class EmailSettings
{
    public const string SectionName = "EmailSettings";
    
    public string FromEmail { get; set; } = null!;
    public string FromName { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
}