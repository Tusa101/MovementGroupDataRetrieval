namespace Shared.Options;

/// <summary>
/// Represents the options for connecting to a Cache database.
/// </summary>
public sealed class RedisConnectionOptions
{
    /// <summary>
    /// The configuration section name for RedisConnectionOptions.
    /// </summary>
    public const string Section = "RedisConnection";

    /// <summary>
    /// Gets or sets the connection string for the Cache database.
    /// </summary>
    /// <value>The connection string.</value>
    public string ConnectionString { get; set; }
}
