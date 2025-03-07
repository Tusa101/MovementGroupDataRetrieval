using Domain.Entities;

namespace Domain.Abstractions.RepositoryInterfaces;
public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
{
    Task<bool> DeleteAllByUserId(Guid userId);
    Task<RefreshToken?> GetByToken(string token, bool includeUser = false);
}
