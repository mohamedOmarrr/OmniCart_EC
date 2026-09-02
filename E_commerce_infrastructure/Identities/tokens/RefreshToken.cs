namespace E_commerce_infrastructure.Identities;

public class RefreshToken
{
    public Guid Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTimeOffset ExpiresAt { get; set; }

    public bool Revoked { get; set; } = false;
    
    public ApplicationUser? User { get; set; }
}