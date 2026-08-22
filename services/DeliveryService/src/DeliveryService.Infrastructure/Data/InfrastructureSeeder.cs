using DeliveryService.Application.Common.Enums;
using DeliveryService.Application.Common.Exceptions;
using DeliveryService.Infrastructure.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace DeliveryService.Infrastructure.Data;

public sealed class InfrastructureSeeder(
    RoleManager<AppRole> roleManager,
    UserManager<AppUser> userManager,
    IConfiguration configuration
)
{
    private static readonly string[] Roles = Enum.GetNames<UserRole>();

    public async Task SeedAsync(CancellationToken ct = default)
    {
        foreach (var roleName in Roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
                await roleManager.CreateAsync(AppRole.Create(roleName));
        }

        var section = configuration.GetSection("IdentitySeed:Driver");
        var username = section["UserName"];
        var email = section["Email"];
        var password = section["Password"];
        if (
            string.IsNullOrWhiteSpace(username)
            || string.IsNullOrWhiteSpace(email)
            || string.IsNullOrWhiteSpace(password)
        )
            return;

        var driver = await userManager.FindByEmailAsync(email);
        if (driver is null)
        {
            driver = AppUser.Create(username, email);
            var result = await userManager.CreateAsync(driver, password);
            if (!result.Succeeded)
                throw new OperationException(
                    string.Join(", ", result.Errors.Select(error => error.Description))
                );
        }

        if (!await userManager.IsInRoleAsync(driver, nameof(UserRole.DeliveryAdmin)))
            await userManager.AddToRoleAsync(driver, nameof(UserRole.DeliveryAdmin));
    }
}
