using Domain.Primitives;

namespace Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
}