namespace E_commerce_infrastructure.Identities;

public record UserTokenData(
        Guid UserId,
        string Email,
        string DisplayName,
        string? Role
    );