using Application.Features.Data.Queries.GetStoredData;
using Application.Utilities.CachingConfiguration.FileSystem;
using Application.Utilities.CachingConfiguration.Redis;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Options;
using Shared.Options;

namespace Application.Services.StoredDataImplementations;
public class FileSystemStoredDataService(IFileSystemCachingProvider cacheHandler, IOptions<CachingOptions> options) : IStoredDataService
{
    private readonly CachingOptions _options = options.Value;
    public SupportedStorage SupportedStorage => SupportedStorage.FileSystem;

    public async Task<Guid> AddStoredData(StoredData storedData)
    {
        if(cacheHandler.FileExists<StoredData>(storedData.Id))
        {
            throw new DuplicateValueException(nameof(StoredData), storedData.Id);
        }
        await cacheHandler.AddToFileSystemCache(storedData, DateTime.UtcNow.AddMinutes(_options.FileSystemExpirationInMinutes));
        
        return storedData.Id;
    }

    public Task<GetStoredDataResponse> GetStoredData(GetStoredDataRequest request)
    {
        throw new NotImplementedException();
    }

    //public async Task DeleteStoredData(Guid id)
    //{
    //    await cacheHandler.DeleteAsync($"{nameof(StoredData)}_{id}");
    //}
}
