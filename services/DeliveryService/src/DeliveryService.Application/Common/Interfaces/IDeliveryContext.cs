using DeliveryService.Domain.Entities.Shipments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DeliveryService.Application.Common.Interfaces;

public interface IDeliveryContext
{
    DbSet<Shipment> Shipments { get; }
    DbSet<DeliveryAttempt> DeliveryAttempts { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
    IEnumerable<EntityEntry> GetTrackedEntries();
}
