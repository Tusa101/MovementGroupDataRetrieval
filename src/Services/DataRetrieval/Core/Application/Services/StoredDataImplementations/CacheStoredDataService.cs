using Application.Features.Data.Queries.GetStoredData;
using Application.Utilities.CachingConfiguration.Redis;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Options;
using Shared.Options;

namespace Application.Services.StoredDataImplementations;
public class CacheStoredDataService(ICacheHandler cacheHandler, IOptions<CachingOptions> options) : IStoredDataService
{
    private readonly CachingOptions _options = options.Value;

    public SupportedStorage SupportedStorage => SupportedStorage.Cache;

    public async Task<Guid> AddStoredDataAsync(StoredData storedData)
    {
        if(await cacheHandler.ExistsAsync($"{nameof(StoredData)}_{storedData.Id}"))
        {
            throw new DuplicateValueException(nameof(StoredData), storedData.Id);
        }

        await cacheHandler.SetAsync($"{nameof(StoredData)}_{storedData.Id}", storedData, TimeSpan.FromMinutes(_options.RedisExpirationInMinutes));
        return storedData.Id;
    }

    public async Task<StoredData> GetStoredDataAsync(Guid id)
    {
        var storedData = await cacheHandler.GetAsync<StoredData>($"{nameof(StoredData)}_{id}");

        return storedData!;
    }

    public async Task<bool> UpdateStoredDataAsync(StoredData storedData)
    {
        await cacheHandler.DeleteAsync($"{nameof(StoredData)}_{storedData.Id}");
        await cacheHandler.SetAsync($"{nameof(StoredData)}_{storedData.Id}", storedData, TimeSpan.FromMinutes(_options.RedisExpirationInMinutes));
        return true;
    }
}
