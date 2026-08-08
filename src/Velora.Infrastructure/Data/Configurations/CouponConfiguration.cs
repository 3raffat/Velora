using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Coupons;
using Velora.Domain.Entities.Customers;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable("Coupons", "velora");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CustomerId).IsRequired();

        builder.Property(c => c.Code).IsRequired().HasMaxLength(50);

        builder
            .Property(c => c.Discount)
            .HasConversion(discount => discount.Amount, value => Money.Create(value))
            .IsRequired();

        builder.Property(c => c.Type).HasConversion<string>().IsRequired();

        builder.Property(c => c.ExpiryDate).IsRequired();

        builder.Property(c => c.IsUsed).IsRequired();

        builder.HasIndex(c => c.Code).IsUnique();

        builder
            .HasOne<Customer>()
            .WithMany()
            .HasForeignKey(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
