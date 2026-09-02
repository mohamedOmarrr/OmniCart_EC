using System.Text.Json;
using E_commerce_application.Interfaces;
using StackExchange.Redis;

namespace E_commerce_infrastructure.Redis;

public class RedisService : IRedisService
{
    private readonly IDatabase _database;

    public RedisService(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null)
    {
        var json = JsonSerializer.Serialize(value);

        if (expiration.HasValue)
        {
            await _database.StringSetAsync(
                key,
                json,
                expiration.Value);
            
        }
        else
        {
            await _database.StringSetAsync(key, json);
        }
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);

        if (!value.HasValue)
            return default;

        return JsonSerializer.Deserialize<T>(value.ToString());
    }

    public async Task DeleteAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _database.KeyExistsAsync(key);
    }
}