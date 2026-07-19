using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;
using Velora.Domain.Entities.Customers;
using Velora.Domain.Entities.Customers.Exceptions;
using Velora.Domain.Entities.ShoppingCart.Exceptions;

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
            throw new RequiredFieldException(nameof(customerId));

        return new Cart(Guid.NewGuid(), customerId);
    }

    public void AddItem(CartItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (IsCheckedOut)
            throw new CartAlreadyCheckedOutException();

        _cartItems.Add(item);
    }

    public void RemoveItem(Guid cartItemId)
    {
        if (IsCheckedOut)
            throw new CartAlreadyCheckedOutException();

        var item = _cartItems.FirstOrDefault(x => x.Id == cartItemId);

        if (item is null)
            throw new CartItemNotFoundException(cartItemId);

        _cartItems.Remove(item);
    }

    public void Checkout()
    {
        if (IsCheckedOut)
            throw new CartAlreadyCheckedOutException();

        if (_cartItems.Count == 0)
            throw new EmptyCartException();

        IsCheckedOut = true;
    }
}
