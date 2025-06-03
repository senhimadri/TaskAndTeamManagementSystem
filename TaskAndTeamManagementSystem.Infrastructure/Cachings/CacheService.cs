using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using TaskAndTeamManagementSystem.Application.Contracts.Infrastructure.Cachings;

namespace TaskAndTeamManagementSystem.Infrastructure.Cachings;

internal class CacheService(IDistributedCache distributedCache) : ICacheService
{
    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await distributedCache.GetStringAsync(key);
        return value is null ? default : JsonSerializer.Deserialize<T>(value);
    }
    public async Task SetAsync<T>(string key, T value, TimeSpan? absoluteExpiration = null)
    {
        var options = new DistributedCacheEntryOptions();
        if (absoluteExpiration.HasValue)
        {
            options.SetAbsoluteExpiration(absoluteExpiration.Value);
        }
        await distributedCache.SetStringAsync(key, JsonSerializer.Serialize(value), options);
    }
    public async Task RemoveAsync(string key)
    {
        await distributedCache.RemoveAsync(key);
    }
}
