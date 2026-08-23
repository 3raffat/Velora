using System.Reflection;
using System.Reflection.Metadata;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Common.Behaviors;
using OrderService.Application.Common.Integrations.Delivery;
using OrderService.Application.Common.Integrations.PayPal;
using OrderService.Application.Common.Interfaces;

namespace OrderService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        IConfiguration cfg
    )
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediataR().AddDeliveryIntegration(cfg).AddPayPalIntegration(cfg);

        return services;
    }

    public static IServiceCollection AddPayPalIntegration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddHttpClient<IPayPalClient, PayPalClient>(client =>
        {
            client.BaseAddress = new Uri(
                configuration["PayPal:BaseUrl"] ?? "https://api-m.sandbox.paypal.com"
            );
        });

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
