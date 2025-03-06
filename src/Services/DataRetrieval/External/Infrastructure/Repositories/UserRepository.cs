using Domain.Abstractions.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Configuration.DataAccess;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories;
public class UserRepository(ApplicationDbContext dbContext) : RepositoryBase<User>(dbContext), IUserRepository
{

}
