using System.ComponentModel.DataAnnotations.Schema;
using Domain.Primitives;

namespace Domain.Entities;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; }
    public DateTime ExpiresOnUtc { get; set; }

    [ForeignKey("UserId")]
    public virtual Guid UserId { get; set; }
    public virtual User User { get; set; }
}