namespace E_commerce_application.Interfaces;

public interface IEmailVerificationCodeStore
{
    string CreateCode();

    Task StoreAsync(
        string userId,
        string code,
        TimeSpan expiration);

    Task<bool> ValidateAsync(
        string userId,
        string code);

    Task RemoveAsync(string userId);
}