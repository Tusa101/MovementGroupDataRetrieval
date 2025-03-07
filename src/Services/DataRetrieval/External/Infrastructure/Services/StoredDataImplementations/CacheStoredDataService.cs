using Domain.Entities;

namespace Infrastructure.Services.StoredDataImplementations;
public class CacheStoredDataService : IStoredDataService
{
    public SupportedStorage SupportedStorage => SupportedStorage.Cache;

    public Task<ICollection<StoredData>> GetStoredData()
    {
        throw new NotImplementedException();
    }
}
