using DeliveryService.Entities.Shipments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryService.Data.Configurations;

public sealed class ShipmentConfiguration : IEntityTypeConfiguration<Shipment>
{
    public void Configure(EntityTypeBuilder<Shipment> builder)
    {
        builder.ToTable("Shipments", "delivery");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.OrderId).IsRequired();

        builder.Property(x => x.TrackingNumber).IsRequired().HasMaxLength(50);

        builder.HasIndex(x => x.TrackingNumber).IsUnique();

        builder.Property(x => x.CustomerName).IsRequired().HasMaxLength(200);

        builder.Property(x => x.CustomerPhone).IsRequired().HasMaxLength(30);

        builder.ComplexProperty(
            x => x.ShippingAddress,
            address =>
            {
                address.Property(x => x.AddressLine1).HasMaxLength(200).IsRequired();

                address.Property(x => x.AddressLine2).HasMaxLength(200);

                address.Property(x => x.City).HasMaxLength(100).IsRequired();

                address.Property(x => x.State).HasMaxLength(100).IsRequired();

                address.Property(x => x.Country).HasMaxLength(100).IsRequired();
            }
        );
        builder.Property(x => x.TotalAmount).HasPrecision(18, 2).IsRequired();

        builder.Property(x => x.Status).HasConversion<string>().IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();

        builder.Property(x => x.DeliveredAt).IsRequired(false);
    }
}
