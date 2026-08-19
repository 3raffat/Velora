using System.Text;
using System.Text.Json.Serialization;
using DeliveryService.Clients;
using DeliveryService.Data;
using DeliveryService.Middleware;
using DeliveryService.Services;
using DeliveryService.Services.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace DeliveryService;

public static class DependencyInjection
{
    public static IServiceCollection AddConfiguration(
        this IServiceCollection services,
        IConfiguration cfg
    )
    {
        services
            .AddDatabase(cfg)
            .AddSwagger()
            .AddExceptionHandling()
            .AddJsonOptions()
            .AddServices()
            .AddHttpClient(cfg)
            .AddAuthenticationService(cfg)
            .AddIdentityConfiguration();
        return services;
    }

    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration cfg
    )
    {
        services.AddDbContext<DeliveryDbContext>(
            (sp, opt) =>
            {
                opt.UseSqlServer(cfg.GetConnectionString("DefaultConnection"));
            }
        );

        services.AddScoped<IDeliveryDbContext>(sp => sp.GetRequiredService<DeliveryDbContext>());

        return services;
    }

    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.AddProblemDetails();

        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }

    public static IServiceCollection AddJsonOptions(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        return services;
    }

    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddDataProtection();

        services
            .AddIdentityCore<AppUser>(opt =>
            {
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = true;
                opt.Password.RequiredLength = 8;
                opt.Password.RequiredUniqueChars = 1;
            })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<DeliveryDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped<IShipmentService, ShipmentService>();
        return services;
    }

    public static IServiceCollection AddHttpClient(
        this IServiceCollection services,
        IConfiguration cfg
    )
    {
        services.AddHttpClient<VeloraClient>(client =>
        {
            client.BaseAddress = new Uri(cfg["Velora:BaseUrl"]!);
        });
        return services;
    }

    public static IServiceCollection AddAuthenticationService(
        this IServiceCollection services,
        IConfiguration cfg
    )
    {
        services
            .AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(opt =>
            {
                var jwtSettings = cfg.GetSection("JwtSettings");

                opt.TokenValidationParameters =
                    new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidAudience = jwtSettings["Audience"],
                        ValidIssuer = jwtSettings["Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
                        ),
                    };
            });
        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo { Title = "Delivery Service API", Version = "v1" }
            );

            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token below (no 'Bearer ' prefix needed).",
                }
            );
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        return services;
    }
}
