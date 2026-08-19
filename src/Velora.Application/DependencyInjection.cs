using System.Reflection;
using System.Reflection.Metadata;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Velora.Application.Common.Behaviors;
using Velora.Application.Common.Integrations.Delivery;
using Velora.Application.Common.Interfaces;

namespace Velora.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration cfg
    )
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediataR().AddDeliveryIntegration(cfg);

        return services;
    }

    public static IServiceCollection AddDeliveryIntegration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddHttpClient<IDeliveryClient, DeliveryClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["DeliveryService:BaseUrl"]!);
        });

        return services;
    }

    public static IServiceCollection AddMediataR(this IServiceCollection services)
    {
        services.AddMediatR(opt =>
        {
            opt.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            opt.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        return services;
    }
}
