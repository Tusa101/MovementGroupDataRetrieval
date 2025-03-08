using Application.Features.Data.Queries.GetStoredData;
using Domain.Entities;

namespace Application.Services.StoredDataImplementations;
public interface IStoredDataService
{
    SupportedStorage SupportedStorage { get; }
    Task<StoredData> GetStoredDataAsync(Guid id);
    Task<Guid> AddStoredDataAsync(StoredData storedData);
    Task<bool> UpdateStoredDataAsync(StoredData storedData);
}
