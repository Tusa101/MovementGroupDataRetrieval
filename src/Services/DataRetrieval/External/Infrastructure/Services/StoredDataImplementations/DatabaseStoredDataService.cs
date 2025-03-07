using Domain.Entities;
using MediatR;

namespace Infrastructure.Services.StoredDataImplementations;
public class DatabaseStoredDataService(ISender sender) : IStoredDataService
{
    public SupportedStorage SupportedStorage => SupportedStorage.Database;

    public Task<ICollection<StoredData>> GetStoredData()
    {
        sender.Send();
    }
}
