using Application.Utilities;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Constants;

namespace Infrastructure.Configuration.DataAccess;
public static class DbContextSeed
{
    #region SeedMethods

    public static async Task SeedRoles(DbSet<Role> dbSet)
    {
        var types = GetPreconfiguredRoles();
        foreach (var type in types)
        {
            if (!await dbSet.AnyAsync(x => x.Id == type.Id))
            {
                await dbSet.AddAsync(type);
            }
        }
    }

    public static async Task SeedUsers(DbSet<User> dbSet)
    {
        var people = GetPreconfiguredUsers();
        foreach (var person in people)
        {
            if (!await dbSet.AnyAsync(x => x.Id == person.Id))
            {
                await dbSet.AddAsync(person);
            }
        }
    }

    public static async Task SeedUserRoles(DbSet<UserRole> dbSet)
    {
        var people = GetPreconfiguredUserRoles();
        foreach (var person in people)
        {
            if (!await dbSet.AnyAsync(x => x.Id == person.Id))
            {
                await dbSet.AddAsync(person);
            }
        }
    }

    #endregion

    #region SeedData

    private static IEnumerable<User> GetPreconfiguredUsers()
    {
        return
        [
            User.Create
            (
                nickName: "admin",
                email: "admin@localhost.com",
                firstName: "admin",
                lastname: "admin",
                passwordHash: PasswordHasher.Hash("Admin_pwd123!"),
                id: new Guid("7a813f1a-2100-4680-9f20-931fa4bffda3")
            ),
            User.Create
            (
                nickName: "user",
                email: "userr@localhost.com",
                firstName: "user",
                lastname: "user",
                passwordHash: PasswordHasher.Hash("user_pwd123!"),
                id: new Guid("f608c5ba-d165-4873-9c8e-bb36e5a0d245")
            )
        ];
    }

    private static IEnumerable<Role> GetPreconfiguredRoles()
    {
        return
        [
            new()
            {
                Id = new Guid("ea17b867-5ed8-4fc1-91e5-6a87b49a364d"),
                Name = ApplicationRoles.Admin
            },
            new()
            {
                Id = new Guid("1ae51b43-6160-4322-80d6-378fcb1ea5a9"),
                Name = ApplicationRoles.User
            },
        ];
    }

    private static IEnumerable<UserRole> GetPreconfiguredUserRoles()
    {
        return
        [
            new()
            {
                UserId = new Guid("7a813f1a-2100-4680-9f20-931fa4bffda3"),
                RoleId = new Guid("ea17b867-5ed8-4fc1-91e5-6a87b49a364d"),
            },
            new()
            {
                UserId = new Guid("f608c5ba-d165-4873-9c8e-bb36e5a0d245"),
                RoleId = new Guid("1ae51b43-6160-4322-80d6-378fcb1ea5a9"),
            },
        ];
    } 

    #endregion
}
