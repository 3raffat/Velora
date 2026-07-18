using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Velora.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Velora.Infrastructure;

public static class DependencyInjection
{


    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration cfg)
    {

        services.AddDbConnection(cfg)
                .AddIdentityConfiguration();


        return services;
    }

    public static IServiceCollection AddDbConnection(this IServiceCollection services, IConfiguration cfg)
    {

        services.AddDbContext<VeloraContext>((sp, opt) =>
        {
            opt.UseSqlServer(cfg.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddDataProtection();

        services.AddIdentityCore<IdentityUser<Guid>>(opt =>
        {
            opt.Password.RequireDigit = true;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireUppercase = true;
            opt.Password.RequireNonAlphanumeric = true;
            opt.Password.RequiredLength = 8;
            opt.Password.RequiredUniqueChars = 1;

            opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
        }).AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<VeloraContext>()
        .AddDefaultTokenProviders();

        return services;
    }
}
