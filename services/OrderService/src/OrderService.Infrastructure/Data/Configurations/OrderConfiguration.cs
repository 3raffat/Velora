using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Common.ValueObjects;
using OrderService.Domain.Entities.Orders;
using PhoneNumbers;

namespace OrderService.Infrastructure.Data.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", "velora");

        builder.HasKey(o => o.Id);

        builder.Property(ci => ci.Id).ValueGeneratedNever();

        builder.Property(o => o.OrderNumber).HasMaxLength(50).IsRequired();

        builder.Property(o => o.OrderDate).IsRequired();

        // builder
        //     .Property(o => o.ShippingCost)
        //     .HasConversion(cost => cost.Amount, value => Money.Create(value))
        //     .IsRequired();

        builder.ComplexProperty(
            p => p.ShippingCost,
            discount =>
            {
                discount.Property(n => n.Amount).IsRequired();
            }
        );

        builder.Property(o => o.OrderStatus).HasConversion<string>().HasMaxLength(50).IsRequired();

        builder.Property(o => o.TotalBaseAmount).HasPrecision(18, 2).IsRequired();

        builder.Property(o => o.TotalDiscountAmount).HasPrecision(18, 2).IsRequired();

        builder.Property(o => o.TotalAmount).HasPrecision(18, 2).IsRequired();
        builder.Property(o => o.DiscountPercentage).HasPrecision(5, 2).IsRequired();

        builder.ComplexProperty(
            o => o.BillingAddress,
            a =>
            {
                a.Property(p => p.AddressLine1)
                    .HasColumnName("BillingAddressLine1")
                    .HasMaxLength(200)
                    .IsRequired();

                a.Property(p => p.AddressLine2)
                    .HasColumnName("BillingAddressLine2")
                    .HasMaxLength(200);

                a.Property(p => p.City).HasColumnName("BillingCity").HasMaxLength(100).IsRequired();

                a.Property(p => p.State)
                    .HasColumnName("BillingState")
                    .HasMaxLength(100)
                    .IsRequired();

                a.Property(p => p.Country)
                    .HasColumnName("BillingCountry")
                    .HasMaxLength(100)
                    .IsRequired();
            }
        );

        builder.ComplexProperty(
            o => o.ShippingAddress,
            a =>
            {
                a.Property(p => p.AddressLine1)
                    .HasColumnName("ShippingAddressLine1")
                    .HasMaxLength(200)
                    .IsRequired();

                a.Property(p => p.AddressLine2)
                    .HasColumnName("ShippingAddressLine2")
                    .HasMaxLength(200);

                a.Property(p => p.City)
                    .HasColumnName("ShippingCity")
                    .HasMaxLength(100)
                    .IsRequired();

                a.Property(p => p.State)
                    .HasColumnName("ShippingState")
                    .HasMaxLength(100)
                    .IsRequired();

                a.Property(p => p.Country)
                    .HasColumnName("ShippingCountry")
                    .HasMaxLength(100)
                    .IsRequired();
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
