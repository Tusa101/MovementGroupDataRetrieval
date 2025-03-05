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
                Title = "Keycloak Tests API",
                Version = "v1",
                Description = "Keycloak Tests API. Created for testing purposes only. Try both explicit and implicit authorization flows"
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

            c.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri(keycloakOptions.AuthorizationUrl),
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "openid" },
                            { "profile", "profile" },
                        }
                    }
                }
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Keycloak",
                            Type = ReferenceType.SecurityScheme,
                        },
                        In = ParameterLocation.Header,
                        Name = "Bearer",
                        Scheme = "Bearer"
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
