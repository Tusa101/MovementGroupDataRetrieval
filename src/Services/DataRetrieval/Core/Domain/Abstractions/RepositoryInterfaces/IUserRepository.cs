using Domain.Entities;

namespace Domain.Abstractions.RepositoryInterfaces;
public interface IUserRepository : IRepositoryBase<User>
{
    Task<User?> GetByEmailAsync(string email, bool includeRoles = false);
}
