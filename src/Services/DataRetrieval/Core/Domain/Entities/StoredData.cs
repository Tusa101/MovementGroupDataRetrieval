using Domain.Primitives;

namespace Domain.Entities;
public class StoredData : BaseEntity
{
    public string Content { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
