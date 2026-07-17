using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Entities.Customers;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.ToTable("Addresses", "velora");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.AddressLine1)
                     .HasMaxLength(200)
                     .IsRequired();

        builder.Property(a => a.AddressLine2)
                      .HasMaxLength(200)
                      .IsRequired();

        builder.Property(a => a.City)
                      .HasMaxLength(100)
                      .IsRequired();

        builder.Property(a => a.State)
                      .HasMaxLength(100)
                      .IsRequired();

        builder.Property(a => a.Country)
                      .HasMaxLength(100)
                      .IsRequired();
    }
}
