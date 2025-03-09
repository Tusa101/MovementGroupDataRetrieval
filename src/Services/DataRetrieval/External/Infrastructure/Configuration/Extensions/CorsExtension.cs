using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Configuration.Extensions;
public static class CorsExtension
{
    private const string SomeFrontendPolicy = "SomeFrontendPolicy";
    private const string AnotherFrontendPolicy = "AnotherFrontendPolicy";

    public static IServiceCollection ConfigureCORS(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy(SomeFrontendPolicy, builder =>
            {
                builder.WithOrigins("http://localhost:4200");
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
            options.AddPolicy(AnotherFrontendPolicy, builder =>
            {
                builder.WithOrigins("http://localhost:4300");
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });
        });

        return services;
    }

    public static WebApplication UseCustomCORS(this WebApplication app)
    {
        app.UseCors(SomeFrontendPolicy);
        app.UseCors(AnotherFrontendPolicy);
        return app;
    }
}
