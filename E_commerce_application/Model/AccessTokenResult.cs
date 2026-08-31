namespace E_commerce_infrastructure.Identities;

public record AccessTokenResult(string Token, DateTimeOffset ExpiresAt);