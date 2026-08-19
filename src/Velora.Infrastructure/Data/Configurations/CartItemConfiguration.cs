using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.ShoppingCart;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems", "velora");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id).ValueGeneratedNever();

        builder.Property(ci => ci.Quantity).IsRequired();

        // builder
        //     .Property(ci => ci.UnitPrice)
        //     .HasConversion(UnitPrice => UnitPrice.Amount, value => Money.Create(value))
        //     .IsRequired();

        builder.ComplexProperty(
            p => p.UnitPrice,
            discount =>
            {
                discount.Property(n => n.Amount).IsRequired();
            }
        );

        builder.Property(ci => ci.Discount).HasPrecision(18, 2).IsRequired();

        builder
            .HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Ignore(ci => ci.TotalPrice);
    }
}
