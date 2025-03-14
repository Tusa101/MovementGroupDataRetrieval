using Application.Utilities.CachingConfiguration.FileSystem;
using Domain.Primitives;
using Microsoft.Extensions.Logging;
using Quartz;

namespace Infrastructure.Jobs;
public class FileSystemCacheManagementJob(
    IFileSystemCachingProvider fileSystemCachingProvider, 
    ILogger<FileSystemCacheManagementJob> logger) : IJob
{
#pragma warning disable CA2254,S2629  // Template should be a static expression, Logging templates should be constant
    public Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("CachingFileSystemManagementJob has been started");
        
        var domainAssembly = typeof(Domain.AssemblyReference).Assembly;

        var entityTypes = domainAssembly.GetTypes().Where(t => t.BaseType == typeof(BaseEntity)).ToList();

        foreach (var entitytype in entityTypes)
        {
            logger.LogInformation($"Checking cache for {entitytype.FullName}");
            if (fileSystemCachingProvider.GetType()
                .GetMethod(nameof(FileSystemCachingProvider.GetFilePathsByType))?
                .MakeGenericMethod(entitytype)?
                .Invoke(fileSystemCachingProvider, null) is not ICollection<string> filePaths)
            {
                logger.LogInformation($"Nothing to clear in cache for {entitytype.FullName}");
                continue;
            }

            logger.LogInformation($"Found {filePaths.Count} files in cache for {entitytype.FullName}");

            var count = 0;

            foreach (var filePath in filePaths)
            {
                var expirationTime = fileSystemCachingProvider.ExtractExpirationFromFilePath(filePath);
                if (expirationTime < DateTime.UtcNow)
                {
                    fileSystemCachingProvider.DeleteFromFileSystemCache(filePath);
                    count++;
                }
            }
            logger.LogInformation($"Removed {count} expired files from cache");
        }

        logger.LogInformation("CachingFileSystemManagementJob has been finished");
#pragma warning restore CA2254, S2629 // Template should be a static expression, Logging templates should be constant
        return Task.CompletedTask;
    }
}
