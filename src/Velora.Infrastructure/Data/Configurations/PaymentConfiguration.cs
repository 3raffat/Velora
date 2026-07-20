using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Orders;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments", "velora");

        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.PaymentMethod)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(50).IsRequired();

        builder.Property(p => p.TransactionId).HasMaxLength(255);

        builder
            .Property(p => p.Amount)
            .HasConversion(payment => payment.Amount, value => Money.Create(value))
            .IsRequired();

        builder.Property(p => p.PaymentDate).IsRequired();

        builder
            .HasOne(p => p.Order)
            .WithOne(o => o.Payment)
            .HasForeignKey<Payment>(p => p.OrderId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
