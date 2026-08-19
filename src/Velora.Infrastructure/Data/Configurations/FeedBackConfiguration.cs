using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Entities.Products;
using Velora.Domain.Entities.Products.ValueObjects;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class FeedBackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.ToTable("Feedbacks", "velora");

        builder.HasKey(f => f.Id);

        builder.Property(ci => ci.Id).ValueGeneratedNever();

        // builder
        //     .Property(f => f.Rating)
        //     .HasConversion(rating => rating.Value, value => Rating.Create(value))
        //     .HasColumnName("Rating")
        //     .IsRequired();

        builder.ComplexProperty(
            p => p.Rating,
            name =>
            {
                name.Property(n => n.Value).HasColumnName("Rating").HasMaxLength(100);
            }
        );

        builder.Property(f => f.Comment).HasMaxLength(500);
    }
}
