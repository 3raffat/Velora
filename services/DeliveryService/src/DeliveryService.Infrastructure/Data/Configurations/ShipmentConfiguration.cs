using DeliveryService.Domain.Entities.Shipments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Infrastructure.Data.Configurations;

public sealed class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.RecipientName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.RecipientPhone).HasMaxLength(50).IsRequired();
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(32).IsRequired();

        builder.OwnsOne(
            x => x.TrackingNumber,
            tracking =>
            {
                tracking
                    .Property(x => x.Value)
                    .HasColumnName("TrackingNumber")
                    .HasMaxLength(32)
                    .IsRequired();
            }
        );

        builder.OwnsOne(
            x => x.DeliveryAddress,
            address =>
            {
                address.Property(x => x.AddressLine1).HasMaxLength(250).IsRequired();
                address.Property(x => x.AddressLine2).HasMaxLength(250);
                address.Property(x => x.City).HasMaxLength(100).IsRequired();
                address.Property(x => x.State).HasMaxLength(100).IsRequired();
                address.Property(x => x.Country).HasMaxLength(100).IsRequired();
            }
        );

        builder
            .HasMany(x => x.DeliveryAttempts)
            .WithOne()
            .HasForeignKey(x => x.ShipmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
