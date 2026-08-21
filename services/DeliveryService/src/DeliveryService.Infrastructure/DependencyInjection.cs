using System.Text;
using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Infrastructure.Data;
using DeliveryService.Infrastructure.Data.Interceptors;
using DeliveryService.Infrastructure.Extensions;
using DeliveryService.Infrastructure.Services;
using DeliveryService.Infrastructure.Services.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace DeliveryService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddDbConnection(configuration)
            .AddIdentityConfiguration()
            .AddAuthenticationService(configuration)
            .AddServices();

        return services;
    }

    public static IServiceCollection AddDbConnection(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, SoftDeleteInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, PublishDomainEventsInterceptor>();
        services.AddDbContext<DeliveryContext>(
            (serviceProvider, options) =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
                options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            }
        );
        services.AddScoped<IDeliveryContext>(serviceProvider =>
            serviceProvider.GetRequiredService<DeliveryContext>()
        );

        return services;
    }

    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddDataProtection();
        services
            .AddIdentityCore<AppUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<DeliveryContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddAuthenticationService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var settings = configuration.GetSection("JwtSettings");
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = settings["Issuer"],
                    ValidateAudience = true,
                    ValidAudience = settings["Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(settings["Key"]!)
                    ),
                    ClockSkew = TimeSpan.Zero,
                };
            });
        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<InfrastructureSeeder>();
        services.AddSingleton<TimeProvider>(TimeProvider.System);

        return services;
    }

    public static Task InitializeInfrastructureAsync(this WebApplication app)
    {
        return app.Services.InitializeInfrastructureAsync();
    }
}
