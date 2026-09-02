using Microsoft.AspNetCore.Identity;

namespace E_commerce_infrastructure.Identities;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public ApplicationUser() => Id = Guid.NewGuid();

    public string? DisplayName { get; set; }
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}