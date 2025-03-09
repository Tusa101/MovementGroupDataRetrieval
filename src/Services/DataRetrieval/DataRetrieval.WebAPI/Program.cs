using Application.Behaviors;
using DataRetrieval.WebAPI.Middleware;
using Infrastructure.Configuration.Extensions;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Presentation.Mapper;
using Serilog;

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
        var builder = WebApplication.CreateBuilder(args);

        var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;

        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

        builder.Services.AddControllers()
            .AddApplicationPart(presentationAssembly);

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGenWithAuth();

        builder.AddLogging();

        var applicationAssembly = typeof(Application.AssemblyReference).Assembly;

        builder.Services.AddAutoMapper(m =>
        {
            m.AddProfile<AccountMappingProfile>();
            m.AddProfile<DataMappingProfile>();
        });

        builder.Services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssembly(applicationAssembly);
            c.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });

        builder.Services.ConfigureCORS();

        builder.Services.AddHealthChecks();

        builder.Host.UseSerilog();

        builder.Services.AddStoredDataFactory();

        builder.Services.AddQuartzConfiguration();

        // AddAsync custom data caching using Cache
        builder.Services.AddCustomDataCaching(builder.Configuration);

        builder.Services.AddDatabase(builder.Configuration);

        builder.Services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        builder.Services.AddRepositories();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddAuth(builder.Configuration);

        // Build the application
        var app = builder.Build();

        // Map health checks endpoint
        app.MapHealthChecks("/Health");

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "LocalDocker")
        {
            // Enable Swagger UI in development environment
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseOutputCache();

        app.UseExceptionHandler(handler => handler.Run(async context =>
        {
            var exceptionHandler = context.RequestServices.GetRequiredService<IExceptionHandler>();
            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

            if (exception != null)
            {
                await exceptionHandler.TryHandleAsync(context, exception, CancellationToken.None);
            }
        }));

        app.PrepareDatabase();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseCustomCORS();

        // Map controllers
        app.MapControllers();

        // Run the application
        app.Run();
    }
}
