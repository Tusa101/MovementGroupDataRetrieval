﻿using Application.Utilities.CachingConfiguration.FileSystem;
using Application.Utilities.CachingConfiguration.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Options;

namespace Infrastructure.Configuration.Extensions;
/// <summary>
/// Extension methods for adding Redis and Filesystem caching to an IServiceCollection.
/// </summary>
public static class CachingExtension
{
    /// <summary>
    /// Adds Redis and Filesystem caching to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add caching to.</param>
    /// <param name="configuration"></param>
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddCustomDataCaching(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDistributedMemoryCache();

        var redisOptions = configuration
            .GetSection(RedisConnectionOptions.Section)
            .Get<RedisConnectionOptions>();

        services.Configure<CachingOptions>(configuration.GetSection(CachingOptions.Section));
        services.AddStackExchangeRedisOutputCache(options =>
        {
            options.Configuration = redisOptions!.ConnectionString;
            options.InstanceName = "DataRetrievalCachingInstance";
        });

        services.AddScoped<ICacheHandler, CacheHandler>();
        services.AddScoped<IFileSystemCachingProvider, FileSystemCachingProvider>();

        return services;
    }
}
