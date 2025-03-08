using Domain.Primitives;
using Infrastructure.Configuration.CachingPoliciesConfiguration;
using Quartz;

namespace Infrastructure.Jobs;
public class FileSystemCacheManagementJob(IFileSystemCachingProvider fileSystemCachingProvider) : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        var domainAssembly = typeof(Domain.AssemblyReference).Assembly;

        var entityTypes = domainAssembly.GetTypes().Where(t => t.BaseType == typeof(BaseEntity)).ToList();

        

        foreach (var entitytype in entityTypes)
        {
            var filePaths = fileSystemCachingProvider.GetType()
                .GetMethod(nameof(FileSystemCachingProvider.GetFilePathsByType))?
                .MakeGenericMethod([entitytype])?
                .Invoke(fileSystemCachingProvider, null) as ICollection<string>;
            
            foreach (var filePath in filePaths)
            {
                var expirationTime = fileSystemCachingProvider.ExtractExpirationFromFilePath(filePath);
                if (expirationTime < DateTime.UtcNow)
                {
                    fileSystemCachingProvider.DeleteFromFileSystemCache(filePath);
                }
            }
        }

        return Task.CompletedTask;
    }
}
