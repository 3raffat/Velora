using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Customers;
using Velora.Domain.Entities.Customers.ValueObjects;

namespace Velora.Infrastructure.Data.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
       public void Configure(EntityTypeBuilder<Customer> builder)
       {
              builder.ToTable("Customers", "velora");

              builder.HasKey(c => c.Id);

              builder.Property(c => c.FirstName)
                      .HasConversion(
                          firstName => firstName.Value,
                          value => Name.Create(value)
                      )
                      .HasMaxLength(100)
                      .IsRequired();

              builder.Property(c => c.LastName)
                     .HasConversion(
                         lastName => lastName.Value,
                         value => Name.Create(value)
                     )
                     .HasMaxLength(100)
                     .IsRequired();

              builder.Property(c => c.PhoneNumber)
                     .HasConversion(
                         phoneNumber => phoneNumber.Value,
                         value => PhoneNumber.Create(value)
                     )
                     .HasMaxLength(100)
                     .IsRequired();

              builder.Property(c => c.Email)
                      .HasConversion(
                              email => email.Value,
                              value => Email.Create(value)
                          ).HasColumnName("Email")
                          .HasMaxLength(225)
                          .IsRequired();

              builder.Property(c => c.PhoneNumber)
                         .IsRequired()
                         .HasMaxLength(30);

              builder.Property(c => c.DateOfBirth)
                          .IsRequired();

              builder.Property(c => c.IsProfileCompleted)
                          .IsRequired();


              builder.HasMany(o => o.Orders)
                          .WithOne(c => c.Customer)
                          .HasForeignKey(o => o.CustomerId)
                          .OnDelete(DeleteBehavior.Cascade);

              builder.HasMany(a => a.Addresses)
                          .WithOne(c => c.Customer)
                          .HasForeignKey(a => a.CustomerId)
                          .OnDelete(DeleteBehavior.Cascade);

              builder.HasMany(cr => cr.Carts)
                          .WithOne(c => c.Customer)
                          .HasForeignKey(cr => cr.CustomerId)
                          .OnDelete(DeleteBehavior.Cascade);

              builder.HasMany(f => f.Feedbacks)
                          .WithOne(c => c.Customer)
                          .HasForeignKey(f => f.CustomerId)
                          .OnDelete(DeleteBehavior.Cascade);

              builder.Navigation(o => o.Orders)
                     .UsePropertyAccessMode(PropertyAccessMode.Field);

              builder.Navigation(a => a.Addresses)
                     .UsePropertyAccessMode(PropertyAccessMode.Field);

              builder.Navigation(cr => cr.Carts)
                     .UsePropertyAccessMode(PropertyAccessMode.Field);

              builder.Navigation(f => f.Feedbacks)
                     .UsePropertyAccessMode(PropertyAccessMode.Field);
       }
}
