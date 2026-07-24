using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Orders;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class RefundConfiguration : IEntityTypeConfiguration<Refund>
{
    public void Configure(EntityTypeBuilder<Refund> builder)
    {
        builder.ToTable("Refunds", "velora");

        builder.HasKey(r => r.Id);

        builder.Property(ci => ci.Id).ValueGeneratedNever();

        builder
            .Property(r => r.Amount)
            .HasConversion(refund => refund.Amount, value => Money.Create(value))
            .IsRequired();

        builder.Property(r => r.Status).HasConversion<string>().HasMaxLength(50).IsRequired();

        builder.Property(r => r.RefundMethod).HasConversion<string>().HasMaxLength(50).IsRequired();

        builder.Property(r => r.RefundReason).HasMaxLength(1000);

        builder.Property(r => r.TransactionId).HasMaxLength(255);

        builder
            .HasOne(r => r.Payment)
            .WithOne(p => p.Refund)
            .HasForeignKey<Refund>(r => r.PaymentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(c => c.Cancellation)
            .WithOne(p => p.Refund)
            .HasForeignKey<Refund>(r => r.CancellationId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(r => r.ProcessedBy);
    }
}
