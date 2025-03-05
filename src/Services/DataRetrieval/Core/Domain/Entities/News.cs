using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Primitives;

namespace Domain.Entities;
public class News : BaseEntity
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    public DateTime PublishedAt { get; set; }

    public int AuthorId { get; set; }

    [ForeignKey($"{nameof(AuthorId)}")]
    public UserProfile Author { get; set; }

    public bool IsPublished { get; set; }
}
