using DeliveryService.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace DeliveryService.Data.Seeding;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<AppUser>>();
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

        const string roleName = "Admin";
        const string email = "admin@velora.com";
        const string username = "admin";
        const string password = "Admin@123456";

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var role = AppRole.Create(roleName);

            var roleResult = await roleManager.CreateAsync(role);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", roleResult.Errors.Select(x => x.Description))
                );
            }
        }

        var admin = await userManager.FindByEmailAsync(email);

        if (admin is null)
        {
            admin = AppUser.Create(username, email);

            admin.EmailConfirmed = true;

            var userResult = await userManager.CreateAsync(admin, password);

            if (!userResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", userResult.Errors.Select(x => x.Description))
                );
            }
        }

        if (!await userManager.IsInRoleAsync(admin, roleName))
        {
            var roleResult = await userManager.AddToRoleAsync(admin, roleName);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", roleResult.Errors.Select(x => x.Description))
                );
            }
        }
    }
}
