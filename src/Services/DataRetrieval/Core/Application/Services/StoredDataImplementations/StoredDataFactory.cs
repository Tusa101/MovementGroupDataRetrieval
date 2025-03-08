namespace Application.Services.StoredDataImplementations;
public class StoredDataFactory(IEnumerable<IStoredDataService> services)
{
    public IStoredDataService GetStoredDataService(SupportedStorage storage)
    {
        return services.FirstOrDefault(x => x.SupportedStorage == storage)
            ?? throw new NotSupportedException($"Storage {storage} is not supported");
    }
}
