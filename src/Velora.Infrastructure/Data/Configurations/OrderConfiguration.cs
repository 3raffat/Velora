using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Orders;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders", "velora");

        builder.HasKey(o => o.Id);

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

        builder
            .HasOne(o => o.ShippingAddress)
            .WithMany()
            .HasForeignKey(o => o.ShippingAddressId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(o => o.BillingAddress)
            .WithMany()
            .HasForeignKey(o => o.BillingAddressId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(o => o.OrderItems).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
