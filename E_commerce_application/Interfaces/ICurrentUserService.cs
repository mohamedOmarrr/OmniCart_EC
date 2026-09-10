namespace E_commerce_application.Interfaces;

public interface ICurrentUserService
{
    Guid? UserId { get; }
}