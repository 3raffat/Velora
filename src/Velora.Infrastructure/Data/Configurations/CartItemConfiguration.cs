using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Entities.ShoppingCart;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems", "velora");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Quantity)
                    .IsRequired();

        builder.Property(ci => ci.UnitPrice)
                    .HasPrecision(18, 2)
                    .IsRequired();

        builder.Property(ci => ci.Discount)
                    .HasPrecision(18, 2)
                    .IsRequired();

        builder.Ignore(ci => ci.TotalPrice);
    }
}
