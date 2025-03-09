namespace Application.Utilities.CachingConfiguration.Redis;

/// <summary>
/// Represents a handler for caching operations.
/// </summary>
public interface ICacheHandler
{
    /// <summary>
    /// Retrieves a value from the cache by key.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key of the value to retrieve.</param>
    /// <returns>The value if found, otherwise null.</returns>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// Retrieves a range of values from the cache by key.
    /// </summary>
    /// <typeparam name="T">The type of the values to retrieve.</typeparam>
    /// <param name="key">The key of the values to retrieve.</param>
    /// <returns>The values if found, otherwise an empty enumerable.</returns>
    Task<IEnumerable<T>> GetRangeAsync<T>(string key);

    /// <summary>
    /// Sets a value in the cache with the specified key and expiration.
    /// </summary>
    /// <param name="key">The key of the value to set.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="expiration">The expiration time of the value.</param>
    /// <returns>True if the value was set successfully, otherwise false.</returns>
    Task<bool> SetAsync(string key, object value, TimeSpan expiration);

    /// <summary>
    /// Deletes a value from the cache by key.
    /// </summary>
    /// <param name="key">The key of the value to delete.</param>
    /// <returns>True if the value was deleted successfully, otherwise false.</returns>
    Task<bool> DeleteAsync(string key);

    /// <summary>
    /// Checks if a value exists in the cache by key.
    /// </summary>
    /// <param name="key">The key of the value to check.</param>
    /// <returns>True if the value exists, otherwise false.</returns>
    Task<bool> ExistsAsync(string key);
}
