using System.Reflection;
using DeliveryService.Application.Common.Behaviors;
using DeliveryService.Application.Common.Integrations.Order;
using DeliveryService.Application.Common.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services,
        string orderServiceBaseUrl
    )
    {
        services.AddValidators().AddMediatRConfiguration().AddOrderIntegration(orderServiceBaseUrl);

        return services;
    }

    public static IServiceCollection AddOrderIntegration(
        this IServiceCollection services,
        string orderServiceBaseUrl
    )
    {
        services.AddHttpClient<IOrderClient, OrderClient>(client =>
        {
            client.BaseAddress = new Uri(orderServiceBaseUrl);
        });

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        return services;
    }

    public static IServiceCollection AddMediatRConfiguration(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
            configuration
                .RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
                .AddOpenBehavior(typeof(ValidationBehavior<,>))
        );

        return services;
    }
}
