using Application.Utilities.CachingConfiguration.FileSystem;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.Extensions.Options;
using Shared.Options;

namespace Application.Services.StoredDataImplementations;
public class FileSystemStoredDataService(IFileSystemCachingProvider cacheHandler, IOptions<CachingOptions> options) : IStoredDataService
{
    private readonly CachingOptions _options = options.Value;
    public SupportedStorage SupportedStorage => SupportedStorage.FileSystem;

    public async Task<Guid> AddStoredDataAsync(StoredData storedData)
    {
        if (cacheHandler.FileExists<StoredData>(storedData.Id))
        {
            throw new DuplicateValueException(nameof(StoredData), storedData.Id);
        }

        await cacheHandler.AddToFileSystemCacheAsync<StoredData>(storedData,
            DateTime.UtcNow.AddMinutes(_options.FileSystemExpirationInMinutes));

        return storedData.Id;
    }

    public async Task<StoredData> GetStoredDataAsync(Guid id)
        => await cacheHandler.GetFromFileSystemCacheAsync<StoredData>(id);

    public async Task<bool> UpdateStoredDataAsync(StoredData storedData)
    {
        await cacheHandler.DeleteFromFileSystemCacheAsync<StoredData>(storedData.Id);

        return await cacheHandler.AddToFileSystemCacheAsync(storedData, DateTime.UtcNow.AddMinutes(_options.FileSystemExpirationInMinutes));
    }
}
