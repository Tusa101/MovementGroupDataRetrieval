using System.Globalization;
using System.Text.Json;
using Domain.Exceptions;
using Domain.Primitives;

namespace Application.Utilities.CachingConfiguration.FileSystem;
public class FileSystemCachingProvider : IFileSystemCachingProvider
{
    public const string FileSystemCachingFolder = "/data";
    public async Task<bool> AddToFileSystemCacheAsync<T>(T obj, DateTime expirationTime, CancellationToken cancellation = default)
        where T : BaseEntity
    {
        var dirPath = "";
        try
        {
            dirPath = GetDirectoryPath<T>();
        }
        catch (Exception)
        {
            Directory.CreateDirectory(dirPath);
        }

        var pattern = "MM-dd-yyyy-HH-mm-ss-tt";
        var expiration = expirationTime.ToString(pattern, CultureInfo.CreateSpecificCulture("en-US"));

        using var streamWriter = new StreamWriter(Path.Combine(dirPath, $"{obj.Id}_{expiration}.json"));
        var serializedObj = JsonSerializer.Serialize(obj);
        await streamWriter.WriteAsync(serializedObj);

        return true;
    }

    public async Task<T?> GetFromFileSystemCacheAsync<T>(Guid id, CancellationToken cancellationToken = default)
        where T : BaseEntity
    {
        var dirPath = GetDirectoryPath<T>();

        var filePath = GetFilePath<T>(id, dirPath);

        using var streamReader = new StreamReader(filePath);

        var deserializedObj = JsonSerializer.Deserialize<T>(
            await streamReader.ReadToEndAsync(cancellationToken))
            ?? throw new NotFoundException(nameof(T), id);

        return deserializedObj;
    }

    private static string GetDirectoryPath<T>() where T : BaseEntity
    {
        var dirPath = $"{FileSystemCachingFolder}/{nameof(T)}";
        if (!Directory.Exists(dirPath))
        {
            throw new DirectoryNotFoundException(dirPath);
        }

        return dirPath;
    }

    public ICollection<string>? GetFilePathsByType<T>()
        where T : BaseEntity
    {
        var dirPath = GetDirectoryPath<T>();

        return Directory.EnumerateFiles(dirPath).ToList();
    }

    public bool FileExists<T>(Guid id)
        where T : BaseEntity
    {
        var dirPath = GetDirectoryPath<T>();

        var filePath = GetFilePath<T>(id, dirPath);
        return File.Exists(filePath);
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

        var dateTime = ExtractExpirationFromFilePath(filePath) 
            ?? throw new NotFoundException(nameof(T), id);

        return filePath;
    }

    public Task DeleteFromFileSystemCacheAsync<T>(Guid id, CancellationToken cancellation = default) 
        where T : BaseEntity
    {
        var dirPath = GetDirectoryPath<T>();

        var filePath = GetFilePath<T>(id, dirPath);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        return Task.CompletedTask;
    }
}
