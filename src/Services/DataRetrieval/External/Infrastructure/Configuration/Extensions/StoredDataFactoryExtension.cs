using Application.Services.StoredDataImplementations;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration.Extensions;
public static class StoredDataFactoryExtension
{
    public static IServiceCollection AddStoredDataFactory(this IServiceCollection services)
    {
        services.AddTransient<IStoredDataService, CacheStoredDataService>();
        services.AddTransient<IStoredDataService, DatabaseStoredDataService>();
        services.AddTransient<IStoredDataService, FileSystemStoredDataService>();
        services.AddTransient<StoredDataFactory>();
        return services;
    }
}
