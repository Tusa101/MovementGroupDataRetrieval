using System.Data;
using Domain.Abstractions;
using Domain.Entities;
using Infrastructure.Configuration.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Configuration.Extensions;
public static class DatabasePreparationConfiguration
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork>(factory =>
            factory.GetRequiredService<ApplicationDbContext>());

        services.AddDbContext<ApplicationDbContext>(o =>
            o.UseNpgsql(configuration.GetRequiredSection("PostgresConnection:ConnectionString").Value!));

        services.AddScoped<IDbConnection>(
                factory => factory.GetRequiredService<ApplicationDbContext>().Database.GetDbConnection());
        return services;
    }

    public static WebApplication PrepareDatabase(this WebApplication app)
    {
        var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStarted.Register(async () =>
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            await db.Database.MigrateAsync();

            await db.Database.BeginTransactionAsync(new CancellationToken());
            try
            {
                await DbContextSeed.SeedUsers(db.Set<User>());
                await DbContextSeed.SeedRoles(db.Set<Role>());
                await DbContextSeed.SeedUserRoles(db.Set<UserRole>());

                await db.SaveChangesAsync();
                await db.Database.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await db.Database.RollbackTransactionAsync();
                throw;
            }
        });
        return app;
    }
}
