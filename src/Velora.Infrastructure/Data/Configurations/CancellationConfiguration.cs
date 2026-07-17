using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Entities.Orders;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class CancellationConfiguration : IEntityTypeConfiguration<Cancellation>
{
    public void Configure(EntityTypeBuilder<Cancellation> builder)
    {
        builder.ToTable("Cancellations", "velora");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Reason)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.RequestedAt)
            .IsRequired();

        builder.Property(c => c.ProcessedAt);

        builder.Property(c => c.ProcessedBy);

        builder.Property(c => c.OrderAmount)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(c => c.CancellationCharges)
            .HasPrecision(18, 2);

        builder.Property(c => c.Remarks)
            .HasMaxLength(1000);


    }
}
