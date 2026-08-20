using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Domain.Common.ValueObjects;
using OrderService.Domain.Entities.Customers;
using OrderService.Domain.Entities.Customers.ValueObjects;

namespace OrderService.Infrastructure.Data.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers", "velora");

        builder.HasKey(c => c.Id);

        builder.Property(ci => ci.Id).ValueGeneratedNever();

        // builder
        //     .Property(c => c.FirstName)
        //     .HasConversion(
        //         firstName => firstName == null ? null : firstName.Value,
        //         value => value == null ? null : Name.Create(value)
        //     )
        //     .HasMaxLength(100);

        builder.ComplexProperty(
            p => p.FirstName,
            name =>
            {
                name.Property(n => n.Value).HasMaxLength(100);
            }
        );

        // builder
        //     .Property(c => c.LastName)
        //     .HasConversion(
        //         lastName => lastName == null ? null : lastName.Value,
        //         value => value == null ? null : Name.Create(value)
        //     )
        //     .HasMaxLength(100);

        builder.ComplexProperty(
            p => p.LastName,
            name =>
            {
                name.Property(n => n.Value).HasMaxLength(100);
            }
        );

        // builder
        //     .Property(c => c.PhoneNumber)
        //     .HasConversion(
        //         phoneNumber => phoneNumber == null ? null : phoneNumber.Value,
        //         value => value == null ? null : PhoneNumber.Create(value)
        //     )
        //     .HasMaxLength(100);

        builder.ComplexProperty(
            p => p.PhoneNumber,
            name =>
            {
                name.Property(n => n.Value).HasMaxLength(100);
            }
        );

        // builder
        //     .Property(c => c.Email)
        //     .HasConversion(
        //         email => email == null ? null : email.Value,
        //         value => value == null ? null : Email.Create(value)
        //     )
        //     .HasColumnName("Email")
        //     .HasMaxLength(225);

        builder.ComplexProperty(
            p => p.Email,
            name =>
            {
                name.Property(n => n.Value).HasColumnName("Email").HasMaxLength(100);
            }
        );

        builder.Property(c => c.DateOfBirth).IsRequired();

        builder.Property(c => c.IsProfileCompleted).IsRequired();

        builder
            .HasMany(o => o.Orders)
            .WithOne(c => c.Customer)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(a => a.Addresses)
            .WithOne(c => c.Customer)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(cr => cr.Carts)
            .WithOne(c => c.Customer)
            .HasForeignKey(cr => cr.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(f => f.Feedbacks)
            .WithOne(c => c.Customer)
            .HasForeignKey(f => f.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(o => o.Orders).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(a => a.Addresses).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(cr => cr.Carts).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(f => f.Feedbacks).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
