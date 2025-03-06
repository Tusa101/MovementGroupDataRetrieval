using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Configuration.DataAccess;
using Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;
public class RefreshTokenRepository(ApplicationDbContext dbContext) :
    RepositoryBase<RefreshToken>(dbContext),
    IRefreshTokenRepository
{
    public async Task<bool> DeleteAllByUserId(Guid userId)
    {
        await _dbSet.Where(rt => rt.UserId == userId).ExecuteDeleteAsync();
        
        return true;
    }
}
