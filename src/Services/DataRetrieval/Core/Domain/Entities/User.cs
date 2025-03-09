using System.ComponentModel.DataAnnotations;
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

    public virtual ICollection<UserRole> UserRoles { get; set; } = [];
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];

    #endregion

    #region Methods
    public static User Create(
    string nickName,
    string email,
    string? firstName,
    string? lastname,
    string passwordHash,
    Guid? id = null)
    {
        return new User
        {
            NickName = nickName,
            Email = email,
            FirstName = firstName,
            LastName = lastname,
            PasswordHash = passwordHash,
            RegistrationDate = DateTimeOffset.UtcNow,
            Id = id ?? Guid.NewGuid()
        };
    }
    #endregion
}
