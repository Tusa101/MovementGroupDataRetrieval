
using System;
using System.Data;
using System.Text;
using Application.Behaviors;
using DataRetrieval.WebAPI.Middleware;
using Domain.Abstractions;
using Domain.Abstractions.RepositoryInterfaces;
using Infrastructure.Configuration.DataAccess;
using Infrastructure.Configuration.Extensions;
using Infrastructure.Configuration.Options;
using Infrastructure.Repositories;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Presentation.Mapper;

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
        // CreateToken a new WebApplication builder
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var presentationAssembly = typeof(Presentation.AssemblyReference).Assembly;

        // Add controllers from the presentation assembly
        builder.Services.AddControllers()
            .AddApplicationPart(presentationAssembly);

        // Add Swagger/OpenAPI configuration
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGenWithAuth();

        var applicationAssembly = typeof(Application.AssemblyReference).Assembly;

        builder.Services.AddAutoMapper(m => m.AddProfile<ApplicationMappingProfile>());

        builder.Services.AddMediatR(c =>
        {
            c.RegisterServicesFromAssembly(applicationAssembly);
            c.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });

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
        
        builder.Services.AddSingleton<IExceptionHandler, GlobalExceptionHandler>();

        builder.Services.AddScoped<IUnitOfWork>(factory => 
            factory.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<IDbConnection>(
                factory => factory.GetRequiredService<ApplicationDbContext>().Database.GetDbConnection());

        builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        builder.Services.AddRepositories();

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddAuth(builder.Configuration);

        


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

        var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStopping.Register(async() =>
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await db.Database.MigrateAsync();
        });

        app.UseAuthentication();

        app.UseAuthorization();


        // Map controllers
        app.MapControllers();

        // Run the application
        app.Run();
    }
}
