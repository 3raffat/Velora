using System.Reflection;
using System.Reflection.Metadata;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Velora.Application.Common.Behaviors;
using Velora.Application.Common.Interfaces;

namespace Velora.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddMediataR();

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
