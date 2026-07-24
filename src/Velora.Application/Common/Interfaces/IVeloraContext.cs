namespace Velora.Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Velora.Domain.Entities.Customers;
using Velora.Domain.Entities.Orders;
using Velora.Domain.Entities.Products;
using Velora.Domain.Entities.ShoppingCart;

public interface IVeloraContext
{
    public DbSet<Address> Addresses { get; }
    public DbSet<Customer> Customers { get; }
    public DbSet<Order> Orders { get; }
    public DbSet<OrderItem> OrderItems { get; }
    public DbSet<Cancellation> Cancellations { get; }
    public DbSet<Payment> Payments { get; }
    public DbSet<Refund> Refunds { get; }
    public DbSet<Category> Categories { get; }
    public DbSet<Feedback> Feedbacks { get; }
    public DbSet<Product> Products { get; }
    public DbSet<Cart> Carts { get; }
    public DbSet<CartItem> CartItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    IEnumerable<EntityEntry> GetTrackedEntries();
}
