using E_commerce_infrastructure.Identities;

namespace E_commerce_application.Interfaces;

public interface ITokenService
{
    Task<string> CreateTokenAsync(UserTokenData user);
}