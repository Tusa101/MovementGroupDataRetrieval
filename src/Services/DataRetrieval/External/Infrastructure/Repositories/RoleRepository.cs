using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Configuration.DataAccess;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class RoleRepository(ApplicationDbContext dbContext) : 
    RepositoryBase<Role>(dbContext), IRoleRepository
{
    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _dbSet
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Name == name);
    }
}
