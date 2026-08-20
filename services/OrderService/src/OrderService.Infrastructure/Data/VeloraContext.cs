using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OrderService.Application.Common.Interfaces;
using OrderService.Domain.Entities.Coupons;
using OrderService.Domain.Entities.Customers;
using OrderService.Domain.Entities.Orders;
using OrderService.Domain.Entities.Products;
using OrderService.Domain.Entities.ShoppingCart;
using OrderService.Infrastructure.Data.Configurations;
using OrderService.Infrastructure.Services.Models;

namespace OrderService.Infrastructure.Data;

public sealed class VeloraContext : IdentityDbContext<AppUser, AppRole, Guid>, IVeloraContext
{
    public VeloraContext(DbContextOptions<VeloraContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(VeloraContext).Assembly);
        builder.IdentityConfigurationTables();
    }

    public IEnumerable<EntityEntry> GetTrackedEntries()
    {
        return ChangeTracker.Entries();
    }

    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Cancellation> Cancellations => Set<Cancellation>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Refund> Refunds => Set<Refund>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();

    public DbSet<Coupon> Coupons => Set<Coupon>();
}
