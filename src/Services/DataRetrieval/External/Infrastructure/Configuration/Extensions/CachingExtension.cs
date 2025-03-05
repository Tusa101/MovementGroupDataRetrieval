using Infrastructure.Configuration.CachingPoliciesConfiguration;
using Infrastructure.Configuration.CommonConstants;
using Infrastructure.Configuration.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration.Extensions;
/// <summary>
/// Extension methods for adding custom data caching to an IServiceCollection.
/// </summary>
public static class CachingExtension
{
    /// <summary>
    /// Adds custom data caching to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add caching to.</param>
    /// <param name="redisOptions">The RedisConnectionOptions for the cache.</param>
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddCustomDataCaching(
        this IServiceCollection services,
        RedisConnectionOptions redisOptions)
    {
        // Add StackExchangeRedisOutputCache to the IServiceCollection
        services.AddStackExchangeRedisOutputCache(options =>
        {
            options.Configuration = redisOptions.ConnectionString;
            options.InstanceName = "DataRetrievalCachingInstance";
        });

        // Add OutputCache to the IServiceCollection
        services.AddOutputCache(options =>
        {
            // Add a base policy to cache requests that start with "/api/weather"
            options.AddBasePolicy(policy =>
            {
                policy.With(c => c.HttpContext.Request.Path.StartsWithSegments("/api/weather")).Tag("tag-all", "tag-weather");
                policy.Expire(TimeSpan.FromSeconds(10));
                policy.Cache();
            });

            // Add a policy with a custom key prefix that expires after 20 seconds
            options.AddPolicy(CachingPolicies.S20WithCustomKey, policy =>
            {
                policy.SetCacheKeyPrefix("custom-key-prefix");
                policy.Expire(TimeSpan.FromSeconds(20));
            });

            // Add a policy that expires after 30 seconds
            options.AddPolicy(CachingPolicies.S30, policy => policy.Expire(TimeSpan.FromSeconds(30)));

            // Add a policy that expires after 1 hour
            options.AddPolicy(CachingPolicies.H1, policy => policy.Expire(TimeSpan.FromHours(1)));

            // Add a policy that uses PostCachingPolicy.Instance
            options.AddPolicy(CachingPolicies.CachePostCustomPolicy, PostCachingPolicy.Instance);
        });

        return services;
    }
}
