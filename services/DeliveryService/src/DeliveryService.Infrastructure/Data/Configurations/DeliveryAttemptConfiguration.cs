using DeliveryService.Domain.Entities.Shipments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Infrastructure.Data.Configurations;

public sealed class DeliveryAttemptConfiguration : IEntityTypeConfiguration<DeliveryAttempt>
{
    public void Configure(EntityTypeBuilder<DeliveryAttempt> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FailureReason).HasMaxLength(1000).IsRequired();
        builder.HasIndex(x => new { x.ShipmentId, x.AttemptedAt });
    }
}
