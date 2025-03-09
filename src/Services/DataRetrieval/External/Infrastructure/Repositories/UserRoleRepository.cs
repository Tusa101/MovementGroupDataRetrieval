using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Configuration.DataAccess;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories;
public class UserRoleRepository(ApplicationDbContext dbContext) :
    RepositoryBase<UserRole>(dbContext), IUserRoleRepository
{
}
