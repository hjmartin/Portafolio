using Identity.Application.Security;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence
{
    public class IdentitySeeder
    {
        public static async Task SeedAsync(ApplicationDbContext db, IPasswordHasher hasher)
        {
            // 1) Roles
            if (!await db.Roles.AnyAsync())
            {
                db.Roles.AddRange(
                    new Role { Name = "Admin" },
                    new Role { Name = "User" }
                );
                await db.SaveChangesAsync();
            }

            // 2) Admin user
            var adminEmail = "admin@wcc.com";

            var admin = await db.Users.FirstOrDefaultAsync(x => x.Email == adminEmail);
            if (admin is null)
            {
                admin = new User
                {
                    Email = adminEmail,
                    FullName = "WCC Admin",
                    PasswordHash = hasher.Hash("Admin123*"), // cámbiala luego
                    IsActive = true
                };

                db.Users.Add(admin);
                await db.SaveChangesAsync();
            }

            // 3) Asignar rol Admin
            var adminRoleId = await db.Roles
                .Where(r => r.Name == "Admin")
                .Select(r => r.Id)
                .FirstAsync();

            var hasRole = await db.UserRoles.AnyAsync(ur => ur.UserId == admin.Id && ur.RoleId == adminRoleId);
            if (!hasRole)
            {
                db.UserRoles.Add(new UserRole { UserId = admin.Id, RoleId = adminRoleId });
                await db.SaveChangesAsync();
            }
        }
    }
}
