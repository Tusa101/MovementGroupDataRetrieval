using Application.Services;
using Application.Services.StoredDataImplementations;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration.Extensions;
public static class StoredDataFactoryExtension
{
    public static IServiceCollection AddStoredDataFactory(this IServiceCollection services)
    {
        services.AddScoped<IStoredDataService, CacheStoredDataService>();
        services.AddScoped<IStoredDataService, DatabaseStoredDataService>();
        services.AddScoped<IStoredDataService, FileSystemStoredDataService>();
        services.Decorate<IStoredDataService, LoggingStoredDataServiceDecorator>();
        services.AddScoped<IStoredDataFactory, StoredDataFactory>();
        return services;
    }
}
