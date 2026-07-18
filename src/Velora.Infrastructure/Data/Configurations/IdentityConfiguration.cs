using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Velora.Infrastructure.Data.Configurations;

public static class IdentityConfiguration
{
    public static void IdentityConfigurationTabels(this ModelBuilder builder)
    {
        const string schema = "identity";

        builder.Entity<IdentityUser<Guid>>().ToTable("Users", schema);
        builder.Entity<IdentityRole<Guid>>().ToTable("Roles", schema);
        builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", schema);
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", schema);
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", schema);
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims", schema);
        builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens", schema);

    }
}
