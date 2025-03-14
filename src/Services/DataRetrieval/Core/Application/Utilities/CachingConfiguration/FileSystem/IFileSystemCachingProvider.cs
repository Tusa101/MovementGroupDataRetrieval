using Domain.Primitives;

namespace Application.Utilities.CachingConfiguration.FileSystem;
public interface IFileSystemCachingProvider
{
    Task<bool> AddToFileSystemCacheAsync<T>(T obj, DateTime expirationTime, CancellationToken cancellation = default)
        where T : BaseEntity;
    Task<T?> GetFromFileSystemCacheAsync<T>(Guid id, CancellationToken cancellationToken = default)
        where T : BaseEntity;
    Task<ICollection<T>?> GetAllFromFileSystemCacheAsync<T>(CancellationToken cancellationToken = default)
        where T : BaseEntity;
    bool FileExists<T>(Guid id)
        where T : BaseEntity;
    ICollection<string>? GetFilePathsByType<T>()
        where T : BaseEntity;
    Task DeleteFromFileSystemCacheAsync<T>(Guid id, CancellationToken cancellation = default)
        where T : BaseEntity;
    bool DeleteFromFileSystemCache(string filePath);
    DateTime? ExtractExpirationFromFilePath(string filePath);
}
