namespace E_commerce_infrastructure.Payment;

public class PaymobSettings
{
    public const string SectionName = "Paymob";
    
    public string BaseUrl { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public string PublicKey { get; set; } = null!;
    public string NotificationUrl { get; set; } = null!;
    public string RedirectionUrl { get; set; } = null!;
    public string HmacSecret { get; set; } = null!;
    public int IntegrationId { get; set; }
}