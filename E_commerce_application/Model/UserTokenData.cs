namespace E_commerce_application.Model;

public class UserTokenData
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Role { get; set; } = null!;
}