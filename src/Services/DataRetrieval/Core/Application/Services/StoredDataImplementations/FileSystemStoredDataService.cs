using Application.Features.Data.Queries.GetStoredData;
using Application.Utilities.CachingConfiguration.FileSystem;
using Application.Utilities.CachingConfiguration.Redis;
using Domain.Entities;

namespace Application.Services.StoredDataImplementations;
public class FileSystemStoredDataService(IFileSystemCachingProvider cacheHandler) : IStoredDataService
{
    public SupportedStorage SupportedStorage => SupportedStorage.FileSystem;

    public Task<Guid> AddStoredData(StoredData storedData)
    {

        cacheHandler.AddToFileSystemCache
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
