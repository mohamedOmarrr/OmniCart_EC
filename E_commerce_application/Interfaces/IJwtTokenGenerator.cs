using E_commerce_application.Model;
using E_commerce_infrastructure.Identities;

namespace E_commerce_application.Interfaces;

public interface IJwtTokenGenerator
{
    AccessTokenResult GenerateToken(UserTokenData userToken);
}