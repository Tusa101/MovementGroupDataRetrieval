using Domain.Entities;

namespace Infrastructure.Services.StoredDataImplementations;
public class FileSystemStoredDataService : IStoredDataService
{
    public SupportedStorage SupportedStorage => SupportedStorage.FileSystem;

    public Task<ICollection<StoredData>> GetStoredData()
    {
        throw new NotImplementedException();
    }
}
