using E_commerce_infrastructure.Identities;

namespace E_commerce_application.Interfaces;

public interface IJwtTokenGenerator
{
    Task<AccessTokenResult> GenerateToken(UserTokenData userToken);
}