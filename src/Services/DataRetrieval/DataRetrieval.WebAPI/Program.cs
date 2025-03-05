
using Infrastructure.Configuration.Extensions;
using Infrastructure.Configuration.Options;

namespace DataRetrieval.WebAPI;

/// <summary>
/// Represents the entry point of the application.
/// </summary>
public static class Program
{
    /// <summary>
    /// The main entry point of the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static void Main(string[] args)
    {
        // Create a new WebApplication builder
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;

        // Add controllers from the presentation assembly
        builder.Services.AddControllers()
            .AddApplicationPart(presentationAssembly);

        // Add Swagger/OpenAPI configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGenWithAuth();

        // Add health checks
        builder.Services.AddHealthChecks();

        // Get Redis connection options from configuration
        var redisOptions = builder.Configuration
            .GetSection(RedisConnectionOptions.Section)
            .Get<RedisConnectionOptions>()
            ?? throw new NullReferenceException(
                $"{nameof(RedisConnectionOptions)} object is null. Redis connection options not found");

        // Add custom data caching using Redis
        builder.Services.AddCustomDataCaching(redisOptions);

        // Build the application
        var app = builder.Build();

        // Map health checks endpoint
        app.MapHealthChecks("/Health");

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            // Enable Swagger UI in development environment
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Enable authorization
        app.UseAuthorization();

        // Map controllers
        app.MapControllers();

        // Run the application
        app.Run();
    }
}
