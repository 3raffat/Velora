using Velora.Domain.Common;
using Velora.Domain.Entities.Customers;

namespace Velora.Domain.Entities.ShoppingCart;

public sealed class Cart : AuditableEntity
{
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public bool IsCheckedOut { get; private set; }

    private readonly List<CartItem> _cartItems = new();
    public IReadOnlyCollection<CartItem> CartItems => _cartItems.AsReadOnly();

    private Cart() { }

    private Cart(Guid id, Guid customerId)
        : base(id)
    {
        CustomerId = customerId;
        IsCheckedOut = false;
    }

    public static Cart Create(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("Customer Id is required.", nameof(customerId));

        return new Cart(Guid.NewGuid(), customerId);
    }

    public void AddItem(CartItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (IsCheckedOut)
            throw new InvalidOperationException("Cannot modify a checked out cart.");

        _cartItems.Add(item);
    }

    public void RemoveItem(Guid cartItemId)
    {
        if (IsCheckedOut)
            throw new InvalidOperationException("Cannot modify a checked out cart.");

        var item = _cartItems.FirstOrDefault(x => x.Id == cartItemId);

        if (item is null)
            throw new InvalidOperationException("Cart item was not found.");

        _cartItems.Remove(item);
    }

    public void Checkout()
    {
        if (IsCheckedOut)
            throw new InvalidOperationException("Cart is already checked out.");

        if (_cartItems.Count == 0)
            throw new InvalidOperationException("Cannot checkout an empty cart.");

        IsCheckedOut = true;
    }
}
