using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Products;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", "velora");

        builder.HasKey(c => c.Id);

        builder.Property(ci => ci.Id).ValueGeneratedNever();

        builder.ComplexProperty(
            p => p.Name,
            name =>
            {
                name.Property(n => n.Value).HasColumnName("Name").HasMaxLength(100).IsRequired();
            }
        );
        builder.Property(c => c.Description).HasMaxLength(500);

        builder
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(c => c.Products).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
