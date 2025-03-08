using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Domain.Exceptions;
using Domain.Primitives;
using Microsoft.OpenApi.Writers;

namespace Infrastructure.Configuration.CachingPoliciesConfiguration;
public class FileSystemCachingProvider : IFileSystemCachingProvider
{
    public const string FileSystemCachingFolder = "/data";
    public async Task<bool> AddToFileSystemCache<T>(T obj, DateTime expirationTime, CancellationToken cancellation)
        where T : BaseEntity
    {
        if (!Directory.Exists($"{FileSystemCachingFolder}/{nameof(T)}"))
        {
            Directory.CreateDirectory($"{FileSystemCachingFolder}/{nameof(T)}");
        }
        var pattern = "MM-dd-yyyy-HH-mm-ss-tt";
        var expiration = expirationTime.ToString(pattern, CultureInfo.CreateSpecificCulture("en-US"));

        using var streamWriter = new StreamWriter($"{FileSystemCachingFolder}/{nameof(T)}/{obj.Id}_{expiration}.json");
        var serializedObj = JsonSerializer.Serialize(obj);
        await streamWriter.WriteAsync(serializedObj);

        return true;
    }

    public async Task<T?> GetFromFileSystemCache<T>(Guid id, CancellationToken cancellationToken)
        where T : BaseEntity
    {
        var dirPath = $"{FileSystemCachingFolder}/{nameof(T)}";
        if (!Directory.Exists(dirPath))
        {
            throw new DirectoryNotFoundException($"{FileSystemCachingFolder}/{nameof(T)}");
        }

        var filePath = GetFilePath<T>(id, dirPath);

        using var streamReader = new StreamReader(filePath);

        var deserializedObj = JsonSerializer.Deserialize<T>(
            await streamReader.ReadToEndAsync(cancellationToken))
            ?? throw new NotFoundException(nameof(T), id);

        return deserializedObj;
    }

    public ICollection<string>? GetFilePathsByType<T>()
    {
        var dirPath = $"{FileSystemCachingFolder}/{nameof(T)}";
        if (!Directory.Exists(dirPath))
        {
            throw new DirectoryNotFoundException(dirPath);
        }

        return Directory.EnumerateFiles(dirPath).ToList();
    }

    public bool DeleteFromFileSystemCache(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return true;
        }
        return false;
    }

    public DateTime? ExtractExpirationFromFilePath(string filePath)
    {
        var timestamp = filePath.Substring(
            startIndex: filePath.LastIndexOf('_') + 1,
            length: filePath.LastIndexOf('.') - filePath.LastIndexOf('_') - 1);

        var pattern = "MM-dd-yyyy-HH-mm-ss-tt";

        DateTime? dateTime = DateTime.TryParseExact(
            timestamp,
            format: pattern,
            provider: CultureInfo.CreateSpecificCulture("en-US"),
            style: DateTimeStyles.None,
            out var result) ? result : null;

        return dateTime;
    }

    private string GetFilePath<T>(Guid id, string dirPath) where T : BaseEntity
    {
        DirectoryInfo directory = new(dirPath);

        var filePath = directory.EnumerateFiles()
            .Where(fi => fi.Name.Contains(id.ToString()))
            .Select(fi => fi.FullName)
            .SingleOrDefault() ?? throw new NotFoundException(nameof(T), id);

        var dateTime = ExtractExpirationFromFilePath(filePath);

        if (dateTime is null)
        {
            File.Delete(dirPath);
            throw new NotFoundException(nameof(T), id);
        }

        if (dateTime < DateTime.UtcNow)
        {
            File.Delete(filePath);
            throw new NotFoundException(nameof(T), id);
        }

        return filePath;
    }
}
