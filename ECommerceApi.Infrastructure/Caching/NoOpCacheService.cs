using ECommerceApi.Application.Common.Interfaces;

namespace ECommerceApi.Infrastructure.Caching;

/// <summary>
/// Fallback cache implementation used when Redis is unavailable.
/// All operations are no-ops — the app works normally, just without caching.
/// </summary>
public class NoOpCacheService : ICacheService
{
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        => Task.FromResult<T?>(default);

    public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default)
        => Task.CompletedTask;
}
