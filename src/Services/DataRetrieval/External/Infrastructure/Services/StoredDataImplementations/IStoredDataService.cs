using Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Services.StoredDataImplementations;
public interface IStoredDataService
{
    SupportedStorage SupportedStorage { get; }
    Task<StoredData> GetStoredData(GetStoredDataRequest);
    Task<Guid> AddStoredData(StoredData storedData);
}
