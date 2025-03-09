namespace Application.Services.StoredDataImplementations;
public interface IStoredDataFactory
{
    IStoredDataService GetStoredDataService(SupportedStorage storage);
}
