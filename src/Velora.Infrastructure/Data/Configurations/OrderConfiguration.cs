using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PhoneNumbers;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Orders;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", "velora");

        builder.HasKey(o => o.Id);

        builder.Property(ci => ci.Id).ValueGeneratedNever();

        builder.Property(o => o.OrderNumber).HasMaxLength(50).IsRequired();

        builder.Property(o => o.OrderDate).IsRequired();

        builder
            .Property(o => o.ShippingCost)
            .HasConversion(cost => cost.Amount, value => Money.Create(value))
            .IsRequired();

        builder.Property(o => o.OrderStatus).HasConversion<string>().HasMaxLength(50).IsRequired();

        builder.Ignore(o => o.TotalBaseAmount);

        builder.Ignore(o => o.TotalDiscountAmount);

        builder.Ignore(o => o.TotalAmount);
        builder.OwnsOne(
            o => o.BillingAddress,
            a =>
            {
                a.Property(p => p.AddressLine1)
                    .HasColumnName("AddressLine1")
                    .HasMaxLength(200)
                    .IsRequired();

                a.Property(p => p.AddressLine2).HasColumnName("AddressLine2").HasMaxLength(200);

                a.Property(p => p.City).HasColumnName("City").HasMaxLength(100).IsRequired();

                a.Property(p => p.State).HasColumnName("State").HasMaxLength(100).IsRequired();

                a.Property(p => p.Country).HasColumnName("Country").HasMaxLength(100).IsRequired();
            }
        );

        builder.OwnsOne(
            o => o.ShippingAddress,
            a =>
            {
                a.Property(p => p.AddressLine1)
                    .HasColumnName("AddressLine1")
                    .HasMaxLength(200)
                    .IsRequired();

                a.Property(p => p.AddressLine2).HasColumnName("AddressLine2").HasMaxLength(200);

                a.Property(p => p.City).HasColumnName("City").HasMaxLength(100).IsRequired();

                a.Property(p => p.State).HasColumnName("State").HasMaxLength(100).IsRequired();

                a.Property(p => p.Country).HasColumnName("Country").HasMaxLength(100).IsRequired();
            }
        );

        builder
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(o => o.OrderItems).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
