using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Products;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products", "velora");

        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Name)
            .HasConversion(name => name.Value, value => Name.Create(value))
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Description).HasMaxLength(1000).IsRequired();

        builder
            .Property(p => p.Price)
            .HasConversion(price => price.Amount, value => Money.Create(value));

        builder.Property(p => p.StockQuantity).IsRequired();

        builder.Property(p => p.ImageUrl).HasMaxLength(500);

        builder.Property(p => p.IsAvailable).IsRequired();

        builder
            .HasMany(p => p.OrderItems)
            .WithOne(oi => oi.Product)
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(p => p.Feedbacks)
            .WithOne(f => f.Product)
            .HasForeignKey(f => f.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
