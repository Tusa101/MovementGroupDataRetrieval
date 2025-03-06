using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Infrastructure.Configuration.Extensions;

/// <summary>
/// Extension methods for adding SwaggerGen with authentication to an IServiceCollection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds SwaggerGen with authentication to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add SwaggerGen with authentication to.</param>
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddSwaggerGenWithAuth(
        this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DataRetrieval API",
                Version = "v1",
                Description = "DataRetrieval API." +
                ".NET Web API that provides a data retrieval service while using caching, file storage, and a database. " +
                "The service follows a layered architecture with design patterns and security mechanisms."
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        []
                    }
                });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });
        return services;
    }
}
