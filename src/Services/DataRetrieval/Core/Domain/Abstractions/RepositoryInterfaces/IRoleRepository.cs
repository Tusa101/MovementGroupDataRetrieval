using Domain.Entities;

namespace Domain.Abstractions.RepositoryInterfaces;
public interface IRoleRepository : IRepositoryBase<Role>
{
    Task<Role?> GetByNameAsync(string name);
}
