using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration.DataAccess;
public class ApplicationDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
