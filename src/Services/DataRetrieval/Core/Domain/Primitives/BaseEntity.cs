using System.ComponentModel.DataAnnotations;

namespace Domain.Primitives;
public abstract class BaseEntity
{
    protected BaseEntity() { }
    protected BaseEntity(Guid id) => Id = id;
    [Key]
    public Guid Id { get; protected set; }
}


