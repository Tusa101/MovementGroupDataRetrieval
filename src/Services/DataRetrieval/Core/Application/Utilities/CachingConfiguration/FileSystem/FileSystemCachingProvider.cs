using System.Globalization;
using System.Text.Json;
using Domain.Exceptions;
using Domain.Primitives;
using Microsoft.Extensions.Options;
using Shared.Options;

namespace Application.Utilities.CachingConfiguration.FileSystem;
public class FileSystemCachingProvider(IOptions<CachingOptions> options) : IFileSystemCachingProvider
{
    private readonly CachingOptions _options = options.Value;

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
            Directory.CreateDirectory(Path.Combine(_options.FileSystemCacheStoragePath, typeof(T).Name));
        }

        var pattern = "MM-dd-yyyy-HH-mm-ss-tt";
        var expiration = expirationTime.ToString(pattern, CultureInfo.CreateSpecificCulture("en-US"));

        var filePath = Path.Combine(dirPath, $"{obj.Id}_{expiration}.json");

        using var streamWriter = new StreamWriter(filePath);

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
            ?? throw new NotFoundException(typeof(T).Name, id);

        return deserializedObj;
    }



    public ICollection<string>? GetFilePathsByType<T>()
        where T : BaseEntity
    {
        try
        {
            var dirPath = GetDirectoryPath<T>();
            return Directory.EnumerateFiles(dirPath).ToList();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public bool FileExists<T>(Guid id)
        where T : BaseEntity
    {
        try
        {
            var dirPath = GetDirectoryPath<T>();
            GetFilePath<T>(id, dirPath);
            return true;
        }
        catch (Exception e) when (e is DirectoryNotFoundException || e is NotFoundException)
        {
            return false;
        }
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

    private string GetDirectoryPath<T>()
        where T : BaseEntity
    {
        var dirPath = Path.Combine(_options.FileSystemCacheStoragePath, typeof(T).Name);
        if (!Directory.Exists(dirPath))
        {
            throw new DirectoryNotFoundException(dirPath);
        }

        return dirPath;
    }

    private string GetFilePath<T>(Guid id, string dirPath)
        where T : BaseEntity
    {
        DirectoryInfo directory = new(dirPath);

        var filePath = directory.EnumerateFiles()
            .Where(fi => fi.Name.Contains(id.ToString()))
            .Select(fi => fi.FullName)
            .SingleOrDefault() 
            ?? throw new NotFoundException(typeof(T).Name, id);

        return filePath;
    }
}
