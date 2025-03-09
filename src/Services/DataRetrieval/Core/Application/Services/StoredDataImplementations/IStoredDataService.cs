using Application.Features.Data.Queries.GetStoredData;
using Domain.Entities;

namespace Application.Services.StoredDataImplementations;
/// <summary>
/// Represents a service for storing and retrieving data.
/// </summary>
public interface IStoredDataService
{
    /// <summary>
    /// Gets the type of storage supported by this service.
    /// </summary>
    SupportedStorage SupportedStorage { get; }

    /// <summary>
    /// Retrieves a stored data item by its ID.
    /// </summary>
    /// <param name="id">The ID of the stored data item.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the retrieved data item.</returns>
    Task<StoredData> GetStoredDataAsync(Guid id);

    /// <summary>
    /// Adds a new stored data item.
    /// </summary>
    /// <param name="storedData">The data item to add.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the added data item.</returns>
    Task<Guid> AddStoredDataAsync(StoredData storedData);

    /// <summary>
    /// Updates an existing stored data item.
    /// </summary>
    /// <param name="storedData">The updated data item.</param>
    /// <returns>A task that represents the asynchronous operation. The task result indicates whether the update was successful.</returns>
    Task<bool> UpdateStoredDataAsync(StoredData storedData);
}
