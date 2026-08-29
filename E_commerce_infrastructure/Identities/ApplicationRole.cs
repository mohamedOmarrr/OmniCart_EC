using Microsoft.AspNetCore.Identity;

namespace E_commerce_infrastructure.Identities;

public sealed class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole() => Id = Guid.NewGuid();

    public ApplicationRole(string roleName) : this() => Name = roleName;

    public string? Description { get; set; }
}