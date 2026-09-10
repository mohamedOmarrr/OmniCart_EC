using System.Security.Claims;
using E_commerce_application.Interfaces;

namespace E_Commerce_persentation.HttpRequests;

public class CurrentUserService(IHttpContextAccessor accessor): ICurrentUserService
{
    public Guid? UserId
    {
        get
        {
            var userId = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            return Guid.TryParse(userId, out var Id) ? Id : null;
        }
    }
}