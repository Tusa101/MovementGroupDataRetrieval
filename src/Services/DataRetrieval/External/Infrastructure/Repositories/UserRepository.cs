using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Configuration.DataAccess;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class UserRepository(ApplicationDbContext dbContext) : RepositoryBase<User>(dbContext), IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, bool includeRoles = false)
    {
        var query = _dbSet.AsNoTracking();

        if (includeRoles)
        {
            query = query
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role);
        }
        return await query.FirstOrDefaultAsync(x => x.Email == email);
    }
}
