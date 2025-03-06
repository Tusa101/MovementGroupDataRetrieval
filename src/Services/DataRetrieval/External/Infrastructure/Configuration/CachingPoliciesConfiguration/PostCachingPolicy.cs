using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace Infrastructure.Configuration.CachingPoliciesConfiguration;
/// <summary>
/// Represents a caching policy for handling POST requests.
/// </summary>
public sealed class PostCachingPolicy : IOutputCachePolicy
{
    /// <summary>
    /// The singleton instance of the <see cref="PostCachingPolicy"/> class.
    /// </summary>
    public static readonly PostCachingPolicy Instance = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="PostCachingPolicy"/> class.
    /// </summary>
    private PostCachingPolicy()
    {
        // Private constructor to enforce singleton pattern
    }

    /// <summary>
    /// Handles the caching of a POST request.
    /// </summary>
    /// <param name="context">The output cache context.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        var attemptOutputCaching = AttemptOutputCaching(context);
        context.EnableOutputCaching = true;
        context.AllowCacheLookup = attemptOutputCaching;
        context.AllowCacheStorage = attemptOutputCaching;
        context.ResponseExpirationTimeSpan = TimeSpan.FromSeconds(10);
        context.AllowLocking = true;

        context.CacheVaryByRules.QueryKeys = "*";

        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Determines whether output caching should be attempted for a given request.
    /// </summary>
    /// <param name="context">The output cache context.</param>
    /// <returns>True if output caching should be attempted, false otherwise.</returns>
    private static bool AttemptOutputCaching(OutputCacheContext context)
    {
        var request = context.HttpContext.Request;

        // Only cache GET, POST, and HEAD requests, by default Microsoft recommends not to cache POST, only GET and HEAD
        if (!HttpMethods.IsGet(request.Method) &&
            !HttpMethods.IsPost(request.Method) &&
            !HttpMethods.IsHead(request.Method))
        {
            return false;
        }

        // Microsoft recommends not to cache data for authorized users, only for non-authorizable methods
        if (StringValues.IsNullOrEmpty(request.Headers.Authorization) ||
            !request.HttpContext.User!.Identity!.IsAuthenticated)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Serves the cached response for a POST request.
    /// </summary>
    /// <param name="context">The output cache context.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Determines whether the response should be served from the cache for a POST request.
    /// </summary>
    /// <param name="context">The output cache context.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public ValueTask ServeResponseAsync(OutputCacheContext context, CancellationToken cancellation)
    {
        var response = context.HttpContext.Response;

        if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie))
        {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }

        if (response.StatusCode != StatusCodes.Status200OK &&
            response.StatusCode != StatusCodes.Status301MovedPermanently)
        {
            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;
        }

        return ValueTask.CompletedTask;
    }
}
