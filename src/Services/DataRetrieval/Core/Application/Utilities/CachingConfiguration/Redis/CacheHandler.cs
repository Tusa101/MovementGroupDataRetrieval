using Domain.CommonConstants;
using Domain.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Application.Utilities.CachingConfiguration.Redis;

/// <summary>
/// Represents a handler for caching operations.
/// </summary>
public class CacheHandler(IDistributedCache cache, ILogger<CacheHandler> logger) : ICacheHandler
{
    private static readonly JsonSerializerSettings _jsonSerializerSettings = new() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

    /// <inheritdoc/>
    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var value = await cache.GetStringAsync(key)
                ?? throw new NotFoundException($"{typeof(T).Name} cannot be found.");

            return System.Text.Json.JsonSerializer.Deserialize<T>(value!);
        }
        catch (Exception ex) when (ex is NotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(LoggingTemplates.CachingExceptionMessageTemplate, ex.ToString());
            return default;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> SetAsync(string key, object value, TimeSpan expiration)
    {
        try
        {
            var json = JsonConvert.SerializeObject(value, _jsonSerializerSettings);
            var options = new DistributedCacheEntryOptions { AbsoluteExpiration = DateTime.UtcNow + expiration };
            await cache.SetStringAsync(key, json, options);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(LoggingTemplates.CachingExceptionMessageTemplate, ex.ToString());
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(string key)
    {
        try
        {
            await cache.RemoveAsync(key);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(LoggingTemplates.CachingExceptionMessageTemplate, ex.ToString());
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsAsync(string key)
    {
        var value = await cache.GetAsync(key);
        if (value is null)
        {
            return false;
        }

        return true;
    }
}
