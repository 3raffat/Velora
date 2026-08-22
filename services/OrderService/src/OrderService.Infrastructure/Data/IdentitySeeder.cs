using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common.Interfaces;
using OrderService.Domain.Entities.Customers;
using OrderService.Infrastructure.Services.Models;

namespace OrderService.Infrastructure.Data;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
        var userManager = services.GetRequiredService<UserManager<AppUser>>();

        foreach (var role in Enum.GetValues<UserRole>())
        {
            var roleName = role.ToString();

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(AppRole.Create(roleName));

                if (!result.Succeeded)
                    throw new InvalidOperationException(
                        $"Failed to create role '{roleName}': {string.Join(", ", result.Errors.Select(error => error.Description))}"
                    );
            }
        }

        var adminEmail =
            configuration["SeedAdmin:Email"]
            ?? throw new InvalidOperationException("SeedAdmin:Email is not configured.");
        var adminPassword =
            configuration["SeedAdmin:Password"]
            ?? throw new InvalidOperationException("SeedAdmin:Password is not configured.");
        var adminUsername = configuration["SeedAdmin:UserName"] ?? "admin";

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin is null)
        {
            admin = AppUser.Create(adminUsername, adminEmail);
            var result = await userManager.CreateAsync(admin, adminPassword);

            if (!result.Succeeded)
                throw new InvalidOperationException(
                    $"Failed to create admin user: {string.Join(", ", result.Errors.Select(error => error.Description))}"
                );
        }

        var adminRole = UserRole.Admin.ToString();

        if (!await userManager.IsInRoleAsync(admin, adminRole))
        {
            var result = await userManager.AddToRoleAsync(admin, adminRole);

            if (!result.Succeeded)
                throw new InvalidOperationException(
                    $"Failed to assign role '{adminRole}' to the admin user: {string.Join(", ", result.Errors.Select(error => error.Description))}"
                );
        }

        if (!admin.EmailConfirmed)
        {
            admin.EmailConfirmed = true;
            var result = await userManager.UpdateAsync(admin);

            if (!result.Succeeded)
                throw new InvalidOperationException(
                    "Failed to confirm the seeded admin user email."
                );
        }

        var context = services.GetRequiredService<IVeloraContext>();
        var customerExists = await context.Customers.AnyAsync(customer =>
            customer.IdentityUserId == admin.Id
        );

        if (!customerExists)
        {
            context.Customers.Add(Customer.Create(admin.Id));
            await context.SaveChangesAsync(CancellationToken.None);
        }
    }
}
