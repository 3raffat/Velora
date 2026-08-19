using System.Reflection;
using DeliveryService.Data.Configurations;
using DeliveryService.Entities.Shipments;
using DeliveryService.Services.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeliveryService.Data;

public interface IDeliveryDbContext
{
    DbSet<Shipment> Shipments { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}

public sealed class DeliveryDbContext
    : IdentityDbContext<AppUser, AppRole, Guid>,
        IDeliveryDbContext
{
    public DeliveryDbContext() { }

    public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DeliveryDbContext).Assembly);
        modelBuilder.IdentityConfigurationTables();
    }

    public DbSet<Shipment> Shipments => Set<Shipment>();
}
