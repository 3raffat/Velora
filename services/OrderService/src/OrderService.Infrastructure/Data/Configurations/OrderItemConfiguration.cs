using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Common.ValueObjects;
using OrderService.Domain.Entities.Orders;

namespace OrderService.Infrastructure.Data.Configurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems", "velora");

        builder.HasKey(oi => oi.Id);

        builder.Property(ci => ci.Id).ValueGeneratedNever();

        builder.Property(oi => oi.Quantity).IsRequired();

        // builder
        //     .Property(oi => oi.UnitPrice)
        //     .HasConversion(order => order.Amount, value => Money.Create(value))
        //     .IsRequired();

        builder.ComplexProperty(
            p => p.UnitPrice,
            discount =>
            {
                discount.Property(n => n.Amount).IsRequired();
            }
        );

        builder.Property(oi => oi.Discount).HasPrecision(18, 2).IsRequired();

        builder.Ignore(oi => oi.TotalPrice);
    }
}
