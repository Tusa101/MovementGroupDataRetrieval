using Application.Features.Data.Queries.GetStoredData;
using Domain.Entities;

namespace Application.Services.StoredDataImplementations;
public interface IStoredDataService
{
    SupportedStorage SupportedStorage { get; }
    Task<GetStoredDataResponse> GetStoredData(GetStoredDataRequest request);
    Task<Guid> AddStoredData(StoredData storedData);
}
