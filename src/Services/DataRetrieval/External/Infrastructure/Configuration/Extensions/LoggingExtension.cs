using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Exceptions;
using Serilog.Settings.Configuration;

namespace Infrastructure.Configuration.Extensions;
public static class LoggingExtension
{
    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {

        var applicationName = Assembly.GetEntryAssembly()?.GetName().Name!;

#pragma warning disable CA1305 // Specify IFormatProvider
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration, new ConfigurationReaderOptions { SectionName = "Logging" })
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", applicationName)
            .Enrich.WithExceptionDetails()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
#pragma warning restore CA1305 // Specify IFormatProvider
        return builder;
    }
}
