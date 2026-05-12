using System.Text.Json;
using ECommerceApi.Application.Common.Interfaces;
using StackExchange.Redis;

namespace ECommerceApi.Infrastructure.Caching;

/// <summary>
/// Redis-backed implementation of ICacheService.
/// Uses StackExchange.Redis directly for full control over key management (prefix scanning, etc.).
/// </summary>
public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _db = redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var value = await _db.StringGetAsync(key);
        if (value.IsNullOrEmpty) return default;
        return JsonSerializer.Deserialize<T>((string)value!, _jsonOptions);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(value, _jsonOptions);
        await _db.StringSetAsync(key, json, expiry ?? TimeSpan.FromMinutes(30));
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _db.KeyDeleteAsync(key);
    }

    public async Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
    {
        // Scan all connected endpoints for matching keys
        var endpoints = _redis.GetEndPoints();
        foreach (var endpoint in endpoints)
        {
            var server = _redis.GetServer(endpoint);
            var keys = server.Keys(pattern: $"{prefix}*").ToArray();
            if (keys.Length > 0)
                await _db.KeyDeleteAsync(keys);
        }
    }
}
