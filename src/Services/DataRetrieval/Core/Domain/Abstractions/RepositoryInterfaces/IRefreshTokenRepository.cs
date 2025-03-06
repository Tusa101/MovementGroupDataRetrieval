using Domain.Entities;

namespace Domain.Abstractions.RepositoryInterfaces;
public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
{
    Task<bool> DeleteAllByUserId(Guid userId);
}
