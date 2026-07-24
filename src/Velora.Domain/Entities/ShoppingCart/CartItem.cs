using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Products;
using Velora.Domain.Entities.Products.Exceptions;
using Velora.Domain.Entities.ShoppingCart.Exceptions;

namespace Velora.Domain.Entities.ShoppingCart;

public sealed class CartItem : AuditableEntity
{
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = null!;
    public decimal Discount { get; private set; }
    public decimal TotalPrice => (UnitPrice.Amount * Quantity) - Discount;

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
        Money unitPrice,
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

    internal static CartItem Create(
        Guid productId,
        Guid cartId,
        int quantity,
        Money unitPrice,
        decimal discount = 0
    )
    {
        if (productId == Guid.Empty)
            throw new RequiredFieldException(nameof(productId));

        if (cartId == Guid.Empty)
            throw new RequiredFieldException(nameof(cartId));

        if (quantity < 1)
            throw new InvalidQuantityException();

        if (discount < 0)
            throw new InvalidDiscountException(discount);

        var total = unitPrice.Amount * quantity;

        if (discount > total)
            throw new InvalidDiscountException(discount, total);

        return new CartItem(Guid.NewGuid(), productId, cartId, quantity, unitPrice, discount);
    }

    public void ChangeQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new InvalidQuantityException();

        Quantity = quantity;
    }
}
