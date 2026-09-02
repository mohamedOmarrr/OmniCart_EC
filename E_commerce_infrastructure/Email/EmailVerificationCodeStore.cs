using System.Security.Cryptography;
using E_commerce_application.Interfaces;

namespace E_commerce_infrastructure.Email;

public class EmailVerificationCodeStore : IEmailVerificationCodeStore
{
    private readonly IRedisService _redis;

    public EmailVerificationCodeStore(
        IRedisService redis)
    {
        _redis = redis;
    }

    public string CreateCode()
    {
        return RandomNumberGenerator
            .GetInt32(100000, 1000000)
            .ToString();
    }

    public async Task StoreAsync(
        string userId,
        string code,
        TimeSpan expiration)
    {
        var key =
            $"email-verification-code:{userId}";

        await _redis.SetAsync(
            key,
            code,
            expiration);
    }

    public async Task<bool> ValidateAsync(
        string userId,
        string code)
    {
        var key =
            $"email-verification-code:{userId}";

        var storedCode =
            await _redis.GetAsync<string>(key);

        return storedCode == code;
    }

    public async Task RemoveAsync(string userId)
    {
        var key =
            $"email-verification-code:{userId}";

        await _redis.DeleteAsync(key);
    }
}