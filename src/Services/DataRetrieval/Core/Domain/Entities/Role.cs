using System.ComponentModel.DataAnnotations;
using Domain.Primitives;

namespace Domain.Entities;
public class Role : BaseEntity
{
    [Required]
    public string Name { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = [];
}
