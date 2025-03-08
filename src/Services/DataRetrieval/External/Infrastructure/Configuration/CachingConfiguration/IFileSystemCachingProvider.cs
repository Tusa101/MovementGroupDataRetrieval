using Domain.Primitives;

namespace Infrastructure.Configuration.CachingPoliciesConfiguration;
public interface IFileSystemCachingProvider
{
    Task<bool> AddToFileSystemCache<T>(T obj, DateTime expirationTime, CancellationToken cancellation = default) 
        where T : BaseEntity;
    Task<T?> GetFromFileSystemCache<T>(Guid id, CancellationToken cancellationToken = default)
        where T : BaseEntity;
    ICollection<string>? GetFilePathsByType<T>();
    bool DeleteFromFileSystemCache(string filePath);
    DateTime? ExtractExpirationFromFilePath(string filePath);
}
