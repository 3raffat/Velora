using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Velora.Application.Common.Interfaces;
using Velora.Domain.Entities.Customers;
using Velora.Domain.Entities.Orders;
using Velora.Domain.Entities.Products;
using Velora.Domain.Entities.ShoppingCart;
using Velora.Infrastructure.Data.Configurations;

namespace Velora.Infrastructure.Data;

public sealed class VeloraContext
    : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>,
        IVeloraContext
{
    public VeloraContext(DbContextOptions<VeloraContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(VeloraContext).Assembly);
        builder.IdentityConfigurationTables();
    }

    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> orderItems => Set<OrderItem>();
    public DbSet<Cancellation> Cancellations => Set<Cancellation>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Refund> Refunds => Set<Refund>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
}
