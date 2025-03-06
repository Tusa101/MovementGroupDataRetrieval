using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Configuration.DataAccess;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories;
public class RefreshTokenRepository(ApplicationDbContext dbContext) : 
    RepositoryBase<RefreshToken>(dbContext), 
    IRefreshTokenRepository
{
}
