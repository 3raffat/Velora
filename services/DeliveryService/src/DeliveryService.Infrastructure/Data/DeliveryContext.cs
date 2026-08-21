using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Domain.Entities.Shipments;
using DeliveryService.Infrastructure.Data.Configurations;
using DeliveryService.Infrastructure.Services.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Infrastructure.Data;

public sealed class DeliveryContext(DbContextOptions<DeliveryContext> options)
    : IdentityDbContext<AppUser, AppRole, Guid>(options),
        IDeliveryContext
{
    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<DeliveryAttempt> DeliveryAttempts => Set<DeliveryAttempt>();

    public IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry> GetTrackedEntries()
    {
        return ChangeTracker.Entries();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(DeliveryContext).Assembly);
        builder.ConfigureIdentityTables();
    }
}
