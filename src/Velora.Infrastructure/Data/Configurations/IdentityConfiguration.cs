using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Velora.Infrastructure.Services.Models;

namespace Velora.Infrastructure.Data.Configurations;

public static class IdentityConfiguration
{
    public static void IdentityConfigurationTables(this ModelBuilder builder)
    {
        const string schema = "identity";
        builder.Entity<AppUser>().Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Entity<AppRole>().Property(x => x.Id).ValueGeneratedOnAdd();

        builder.Entity<AppUser>().ToTable("Users", schema);
        builder.Entity<AppRole>().ToTable("Roles", schema);
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", schema);
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", schema);
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", schema);
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims", schema);
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens", schema);
    }
}
