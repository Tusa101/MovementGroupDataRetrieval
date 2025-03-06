using System.ComponentModel.DataAnnotations;

namespace Domain.Primitives;
public abstract class BaseEntity
{   
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();
}


