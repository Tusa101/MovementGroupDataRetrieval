using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Primitives;

namespace Domain.Entities;
public class User : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string NickName { get; set; }

    [MaxLength(100)]
    public string? FirstName { get; set; }
    
    [MaxLength(100)]
    public string? LastName { get; set; }

    public string? PasswordHash { get; set; }

    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; }

    public ICollection<News> NewsArticles { get; set; } = [];
}
