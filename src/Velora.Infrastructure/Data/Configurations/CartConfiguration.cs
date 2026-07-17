using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Entities.ShoppingCart;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts", "velora");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.IsCheckedOut)
               .IsRequired();

        builder.HasMany(ci => ci.CartItems)
               .WithOne(c => c.Cart)
               .HasForeignKey(ci => ci.CartId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(c => c.CartItems)
               .UsePropertyAccessMode(PropertyAccessMode.Field);

    }
}
