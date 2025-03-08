using Application.Features.Data.Queries.GetStoredData;
using Domain.Entities;

namespace Application.Services.StoredDataImplementations;
public class CacheStoredDataService : IStoredDataService
{
    public SupportedStorage SupportedStorage => SupportedStorage.Cache;

    public Task<Guid> AddStoredData(StoredData storedData)
    {
        throw new NotImplementedException();
    }

    public Task<GetStoredDataResponse> GetStoredData(GetStoredDataRequest request)
    {
        throw new NotImplementedException();
    }
}
