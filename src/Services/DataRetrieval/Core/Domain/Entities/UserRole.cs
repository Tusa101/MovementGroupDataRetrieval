using System.ComponentModel.DataAnnotations.Schema;
using Domain.Primitives;

namespace Domain.Entities;
public class UserRole : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }

    #region Navigation

    public User User { get; set; } = null!;
    public Role Role { get; set; } = null!; 

    #endregion
}
