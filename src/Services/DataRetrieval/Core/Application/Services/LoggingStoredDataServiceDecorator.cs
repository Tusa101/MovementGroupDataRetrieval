using Application.Services.StoredDataImplementations;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2629:Logging templates should be constant", Justification = "<Pending>")]
[System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S6668:Logging arguments should be passed to the correct parameter", Justification = "<Pending>")]
public class LoggingStoredDataServiceDecorator(
    IStoredDataService storedDataService,
    ILogger<LoggingStoredDataServiceDecorator> logger) : IStoredDataService
{
    public const string SuccessLoggingTemplate = "Successfully executed {NameOfMethod} for {Id} with storage {SupportedStorage}";
    public const string FailedLoggingTemplate = "Failed executing {NameOfMethod} for {Id} with storage {SupportedStorage}, {Exception}";

    public SupportedStorage SupportedStorage => storedDataService.SupportedStorage;

    public async Task<Guid> AddStoredDataAsync(StoredData storedData)
    {
        logger.LogInformation($"Adding stored data to storage {SupportedStorage}");
        try
        {
            var result = await storedDataService.AddStoredDataAsync(storedData);
            logger.LogInformation(SuccessLoggingTemplate, nameof(AddStoredDataAsync), storedData.Id, SupportedStorage);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(FailedLoggingTemplate, nameof(AddStoredDataAsync), storedData.Id, SupportedStorage, ex);
            throw;
        }
    }

    public async Task<ICollection<StoredData>> GetAllStoredDataAsync()
    {
        logger.LogInformation($"Retrieving (GetAll) stored data from the storage {SupportedStorage}");
        try
        {
            var result = await storedDataService.GetAllStoredDataAsync();
            logger.LogInformation(SuccessLoggingTemplate, nameof(GetAllStoredDataAsync), nameof(StoredData), SupportedStorage);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(FailedLoggingTemplate, nameof(GetStoredDataAsync), nameof(StoredData), SupportedStorage, ex);
            throw;
        }
    }

    public async Task<StoredData> GetStoredDataAsync(Guid id)
    {
        logger.LogInformation($"Retrieving (Get) stored data from the storage {SupportedStorage}");
        try
        {
            var result = await storedDataService.GetStoredDataAsync(id);
            logger.LogInformation(SuccessLoggingTemplate, nameof(GetStoredDataAsync), id, SupportedStorage);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(FailedLoggingTemplate, nameof(GetStoredDataAsync), id, SupportedStorage, ex);
            throw;
        }
    }

    public async Task<bool> UpdateStoredDataAsync(StoredData storedData)
    {
        logger.LogInformation($"Updating stored data in the storage {SupportedStorage}");
        try
        {
            var result = await storedDataService.UpdateStoredDataAsync(storedData);
            logger.LogInformation(SuccessLoggingTemplate, nameof(UpdateStoredDataAsync), storedData.Id, SupportedStorage);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(FailedLoggingTemplate, nameof(UpdateStoredDataAsync), storedData.Id, SupportedStorage, ex);
            throw;
        }
    }
}
