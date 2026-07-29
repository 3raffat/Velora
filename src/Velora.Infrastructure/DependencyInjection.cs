using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Velora.Application.Common.Interfaces;
using Velora.Infrastructure.Data;
using Velora.Infrastructure.Data.Interceptors;

namespace Velora.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration cfg
    )
    {
        services.AddDbConnection(cfg).AddIdentityConfiguration().AddHangfire(cfg);

        services.AddSingleton<TimeProvider>(TimeProvider.System);
        return services;
    }

    public static IServiceCollection AddDbConnection(
        this IServiceCollection services,
        IConfiguration cfg
    )
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, SoftDeleteInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, PublishDomainEventsInterceptor>();

        services.AddDbContext<VeloraContext>(
            (sp, opt) =>
            {
                opt.UseSqlServer(cfg.GetConnectionString("DefaultConnection"));
                opt.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            }
        );

        services.AddScoped<IVeloraContext>(sp => sp.GetRequiredService<VeloraContext>());

        return services;
    }

    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddDataProtection();

        services
            .AddIdentityCore<IdentityUser<Guid>>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequiredUniqueChars = 1;

                opt.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<VeloraContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddHangfire(
        this IServiceCollection services,
        IConfiguration cfg
    )
    {
        services.AddHangfire(config =>
            config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(cfg.GetConnectionString("DefaultConnection"))
        );

        services.AddHangfireServer();

        return services;
    }
}
