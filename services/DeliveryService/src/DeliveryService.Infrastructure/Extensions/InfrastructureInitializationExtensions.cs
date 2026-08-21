using DeliveryService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryService.Infrastructure.Extensions;

public static class InfrastructureInitializationExtensions
{
    public static async Task InitializeInfrastructureAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DeliveryContext>();
        await context.Database.EnsureCreatedAsync();
        await scope.ServiceProvider.GetRequiredService<InfrastructureSeeder>().SeedAsync();
    }
}
