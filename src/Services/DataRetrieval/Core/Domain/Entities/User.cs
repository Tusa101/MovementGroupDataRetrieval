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

    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [EmailAddress]
    public string Email { get; set; }

    public DateTimeOffset RegistrationDate { get; set; }

    #region Navigation
    
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<News> NewsArticles { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];

    #endregion

    #region Methods
    public static User Create(
    string nickName,
    string email,
    string? firstName,
    string? lastname,
    string passwordHash)
    {
        return new User
        {
            NickName = nickName,
            Email = email,
            FirstName = firstName,
            LastName = lastname,
            PasswordHash = passwordHash,
            RegistrationDate = DateTimeOffset.UtcNow,
        };
    } 
    #endregion
}
