namespace Shared.Options;
public class CachingOptions
{
    public const string Section = "Caching";
    public int RedisExpirationInMinutes { get; set; }
    public int FileSystemExpirationInMinutes { get; set; }
}
