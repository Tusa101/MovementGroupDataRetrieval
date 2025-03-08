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

    public async Task<Guid> AddStoredData(StoredData storedData)
    {
        if(await cacheHandler.ExistsAsync($"{nameof(StoredData)}_{storedData.Id}"))
        {
            throw new DuplicateValueException(nameof(StoredData), storedData.Id);
        }

        await cacheHandler.SetAsync($"{nameof(StoredData)}_{storedData.Id}", storedData, TimeSpan.FromMinutes(_options.RedisExpirationInMinutes));
        return Guid.NewGuid();
    }

    public async Task<GetStoredDataResponse> GetStoredData(GetStoredDataRequest request)
    {
        if (!await cacheHandler.ExistsAsync($"{nameof(StoredData)}_{request.Id}"))
        {
            throw new NotFoundException(nameof(StoredData), request.Id);
        }

        var storedData = await cacheHandler.GetAsync<StoredData>($"{nameof(StoredData)}_{request.Id}");

        return new(storedData!);
    }

    public async Task DeleteStoredData(Guid id)
    {
        await cacheHandler.DeleteAsync($"{nameof(StoredData)}_{id}");
    }
}
