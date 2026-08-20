using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;
using OrderService.Domain.Common.ValueObjects;
using OrderService.Domain.Entities.Customers;
using OrderService.Domain.Entities.Customers.Exceptions;
using OrderService.Domain.Entities.ShoppingCart.Enums;
using OrderService.Domain.Entities.ShoppingCart.Exceptions;

namespace OrderService.Domain.Entities.ShoppingCart;

public sealed class Cart : AuditableEntity
{
    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;
    public CartStatus Status { get; private set; }
    private readonly List<CartItem> _cartItems = new();
    public IEnumerable<CartItem> CartItems => _cartItems.AsReadOnly();

    private Cart() { }

    private Cart(Guid id, Guid customerId)
        : base(id)
    {
        CustomerId = customerId;
        Status = CartStatus.Active;
    }

    public static Cart Create(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new RequiredFieldException(nameof(customerId));

        return new Cart(Guid.NewGuid(), customerId);
    }

    public void AddItem(Guid productId, int quantity, decimal unitPrice, decimal discount = 0)
    {
        if (Status is CartStatus.CheckedOut)
            throw new CartAlreadyCheckedOutException();

        var existProduct = _cartItems.FirstOrDefault(p => p.ProductId == productId);

        if (existProduct is not null)
        {
            existProduct.ChangeQuantity(quantity);
            return;
        }

        var item = CartItem.Create(productId, Id, quantity, Money.Create(unitPrice), discount);

        _cartItems.Add(item);
    }

    public void RemoveItem(Guid productId)
    {
        if (Status is CartStatus.CheckedOut)
            throw new CartAlreadyCheckedOutException();

        var item = _cartItems.FirstOrDefault(x => x.ProductId == productId);

        if (item is null)
            throw new CartItemNotFoundException(productId);

        _cartItems.Remove(item);
    }

    public void Checkout()
    {
        if (Status is CartStatus.CheckedOut)
            throw new CartAlreadyCheckedOutException();

        if (_cartItems.Count == 0)
            throw new EmptyCartException();

        Status = CartStatus.CheckedOut;
    }

    public void UpdateQuantity(Guid productId, int quantity)
    {
        var item = _cartItems.FirstOrDefault(i => i.ProductId == productId);

        if (item is null)
            throw new CartItemNotFoundException(productId);

        item.ChangeQuantity(quantity);
    }

    public void Clear()
    {
        if (!CartItems.Any())
            throw new EmptyCartException();

        _cartItems.Clear();
    }
}
