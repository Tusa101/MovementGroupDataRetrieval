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
        var roles = GetPreconfiguredRoles();
        foreach (var role in roles)
        {
            if (!await dbSet.AnyAsync(x => x.Id == role.Id))
            {
                await dbSet.AddAsync(role);
            }
        }
    }

    public static async Task SeedUsers(DbSet<User> dbSet)
    {
        var users = GetPreconfiguredUsers();
        foreach (var user in users)
        {
            if (!await dbSet.AnyAsync(x => x.Id == user.Id))
            {
                await dbSet.AddAsync(user);
            }
        }
    }

    public static async Task SeedUserRoles(DbSet<UserRole> dbSet)
    {
        var userRoles = GetPreconfiguredUserRoles();
        foreach (var userRole in userRoles)
        {
            if (!await dbSet.AnyAsync(x => x.Id == userRole.Id))
            {
                await dbSet.AddAsync(userRole);
            }
        }
    }

    public static async Task SeedStoredData(DbSet<StoredData> dbSet)
    {
        var dataObjects = GetPreconfiguredStoredData();
        foreach (var dataObject in dataObjects)
        {
            if (!await dbSet.AnyAsync(x => x.Id == dataObject.Id))
            {
                await dbSet.AddAsync(dataObject);
            }
        }
    }

    #endregion

    #region SeedData

    private static IEnumerable<StoredData> GetPreconfiguredStoredData()
    {
        return
        [
            new()
            {
                Id = new Guid("07711066-4265-4ec0-9b64-46395ad908d9"),
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc viverra elit euismod tellus fringilla efficitur. Nulla varius neque velit, in maximus libero rhoncus ut. Proin bibendum pharetra eros eu dignissim. Nam sagittis augue diam, ut tincidunt velit placerat eu. Praesent ut gravida urna. Mauris porta vulputate dapibus. Cras sit amet ex dolor. Nulla sed accumsan est. Curabitur sollicitudin, augue nec venenatis imperdiet, elit lorem gravida orci, vitae placerat urna risus nec ante. Cras efficitur fermentum nibh, a tristique sem luctus vitae. Quisque vel nisi felis." +
                "\r\n\r\nAliquam erat volutpat. Sed placerat nisl ut vehicula sollicitudin. Curabitur et ante justo. Nullam at orci auctor, dignissim ipsum vulputate, finibus nisi. Praesent mi lacus, gravida eget accumsan nec, rhoncus nec augue. Aliquam rutrum mauris eget magna tincidunt tempor et non massa. Praesent lobortis purus ante, ut efficitur neque volutpat in. Mauris eleifend lacus vitae velit lacinia finibus. Nullam vitae ultrices ex. Etiam sem elit, ornare nec elit quis, volutpat bibendum orci. Sed ut tortor eu nunc vehicula pretium. Fusce lacinia rutrum ex sed luctus. Vivamus accumsan non sapien ac egestas. Suspendisse ut dolor nibh. Nam vulputate ligula quis dolor laoreet, nec feugiat quam varius.",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = new Guid("ceb76f5b-6aef-4290-975f-0584363e1bb2"),
                Content = "Nunc vitae justo ultricies, viverra orci sed, finibus purus. Ut eu dictum elit. Integer hendrerit fermentum tortor vel rutrum. Sed imperdiet elementum lectus. Aenean nec aliquet nisi, id tristique odio. Duis sed felis at sapien scelerisque vulputate sit amet id odio. Vestibulum hendrerit dapibus ligula non imperdiet.",
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = new Guid("8d5ccef9-1ca2-41f3-9d55-b96c741d089a"),
                Content = "Quisque ultricies vulputate velit ut porta. Duis sit amet porttitor velit. Mauris nisi leo, tincidunt nec mattis eget, vulputate vel nulla. Integer imperdiet purus massa, et imperdiet leo faucibus porttitor. Donec id vehicula lacus, ac sollicitudin enim. Morbi purus odio, luctus nec neque sit amet, accumsan rutrum diam. In hac habitasse platea dictumst. Cras non ullamcorper turpis. Praesent volutpat mi diam, at fermentum ligula dapibus sed." +
                "\r\n\r\nUt lobortis turpis et aliquam hendrerit. Etiam porta leo vel lacus blandit, mollis tincidunt metus aliquam. Vestibulum quis augue et metus imperdiet rutrum quis eu ipsum. Vivamus auctor arcu non sollicitudin fringilla. Praesent pulvinar vehicula ex, at vehicula nunc hendrerit sit amet. Morbi pulvinar lacinia sapien a scelerisque. Ut a placerat nunc, nec aliquet tortor. Ut eu erat eget leo efficitur rhoncus ac vel leo. Vestibulum finibus velit at neque fringilla, id porttitor sem sodales. Donec et auctor mi. Ut tristique magna sem, bibendum laoreet nisi tincidunt in. Nunc non risus dapibus, gravida turpis vitae, varius justo. Phasellus in justo augue. Vivamus imperdiet lorem et erat dignissim rutrum. Suspendisse aliquet lorem vel nisi feugiat, ut placerat sapien bibendum. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos." +
                "\r\n\r\nCurabitur finibus sagittis sapien non ornare. Mauris fringilla felis ex, rhoncus sagittis odio scelerisque sit amet. Etiam venenatis interdum varius. Cras tincidunt leo id dui blandit, et faucibus nibh consequat. Sed finibus eleifend leo sed hendrerit. Duis at viverra augue. Donec tristique, purus eget convallis rutrum, purus ex volutpat massa, sit amet ullamcorper lorem mauris ac mauris. Etiam eu sagittis sem. Sed ullamcorper tortor dui, quis cursus magna tristique et. Praesent mollis sagittis tortor vel condimentum. Nunc iaculis volutpat ultricies. Integer vitae auctor sem.",
                CreatedAt = DateTime.UtcNow
            },
        ];
    }

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
