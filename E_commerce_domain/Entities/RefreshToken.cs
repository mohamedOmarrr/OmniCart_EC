namespace E_commerce_infrastructure.Identities;

public class RefreshToken
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTimeOffset ExpiresAt { get; set; }

    public bool Revoked { get; set; } = false;
    
    public ApplicationUser? User { get; set; }
}