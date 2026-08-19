using DeliveryService.Services.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Data.Configurations;

public static class IdentityConfiguration
{
    private const string Schema = "identity";

    public static void IdentityConfigurationTables(this ModelBuilder builder)
    {
        builder.Entity<AppUser>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<AppRole>().Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Entity<AppUser>().ToTable("Users", Schema);
        builder.Entity<AppRole>().ToTable("Roles", Schema);
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", Schema);
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", Schema);
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", Schema);
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims", Schema);
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens", Schema);
    }
}
