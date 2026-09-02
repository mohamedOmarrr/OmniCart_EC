namespace E_commerce_application.Interfaces;

public interface IRedisService
{
    Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null);

    Task<T?> GetAsync<T>(string key);

    Task DeleteAsync(string key);

    Task<bool> ExistsAsync(string key);
}