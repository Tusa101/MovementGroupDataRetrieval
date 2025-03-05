namespace Infrastructure.Configuration.CommonConstants;
/// <summary>
/// This class contains constants for caching policies used in the application.
/// </summary>
public static class CachingPolicies
{
    /// <summary>
    /// This constant represents a caching policy with a time-to-live (TTL) of 20 seconds and a custom key.
    /// </summary>
    public const string S20WithCustomKey = "s20withCustomKey";

    /// <summary>
    /// This constant represents a caching policy with a TTL of 30 seconds.
    /// </summary>
    public const string S30 = "s30";

    /// <summary>
    /// This constant represents a custom caching policy for caching POST requests.
    /// </summary>
    public const string CachePostCustomPolicy = "cachePostCustomPolicy";

    /// <summary>
    /// This constant represents a caching policy with a TTL of 1 hour.
    /// </summary>
    public const string H1 = "h1";
}

