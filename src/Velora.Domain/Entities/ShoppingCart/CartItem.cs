using Velora.Domain.Common;
using Velora.Domain.Entities.Products;

namespace Velora.Domain.Entities.ShoppingCart;

public sealed class CartItem : AuditableEntity
{
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalPrice => (UnitPrice * Quantity) - Discount;

    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    public Guid CartId { get; private set; }
    public Cart Cart { get; private set; } = null!;

    private CartItem() { }

    private CartItem(
        Guid id,
        Guid productId,
        Guid cartId,
        int quantity,
        decimal unitPrice,
        decimal discount
    )
        : base(id)
    {
        ProductId = productId;
        CartId = cartId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
    }

    public static CartItem Create(
        Guid productId,
        Guid cartId,
        int quantity,
        decimal unitPrice,
        decimal discount = 0
    )
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("Product Id is required.", nameof(productId));

        if (cartId == Guid.Empty)
            throw new ArgumentException("Cart Id is required.", nameof(cartId));

        if (quantity < 1)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be at least 1.");

        if (unitPrice < 0)
            throw new ArgumentOutOfRangeException(
                nameof(unitPrice),
                "Unit price cannot be negative."
            );

        if (discount < 0)
            throw new ArgumentOutOfRangeException(nameof(discount), "Discount cannot be negative.");

        if (discount > unitPrice * quantity)
            throw new ArgumentOutOfRangeException(
                nameof(discount),
                "Discount cannot exceed the item's total price."
            );

        return new CartItem(Guid.NewGuid(), productId, cartId, quantity, unitPrice, discount);
    }

    public void IncreaseQuantity(int amount)
    {
        if (amount < 1)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be at least 1.");

        Quantity += amount;
    }

    public void DecreaseQuantity(int amount)
    {
        if (amount < 1)
            throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be at least 1.");

        if (Quantity - amount < 1)
            throw new InvalidOperationException(
                "Quantity cannot drop below 1 — remove the item instead."
            );

        Quantity -= amount;
    }
}
