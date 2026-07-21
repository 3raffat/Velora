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

        builder
            .Property(c => c.Name)
            .HasConversion(name => name.Value, value => Name.Create(value))
            .HasColumnName("Name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Description).HasMaxLength(500);

        builder
            .HasMany(c => c.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(c => c.Products).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
