using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;
using OrderService.Domain.Common.ValueObjects;
using OrderService.Domain.Entities.Orders.Exceptions;
using OrderService.Domain.Entities.Products;
using OrderService.Domain.Entities.ShoppingCart.Exceptions;

namespace OrderService.Domain.Entities.Orders;

public sealed class OrderItem : BaseEntity
{
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; } = null!;
    public decimal Discount { get; private set; }
    public decimal TotalPrice => (UnitPrice.Amount * Quantity) - Discount;

    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;

    public Guid ProductId { get; private set; }
    public Product Product { get; private set; } = null!;

    private OrderItem() { }

    private OrderItem(
        Guid id,
        Guid orderId,
        Guid productId,
        int quantity,
        Money unitPrice,
        decimal discount
    )
        : base(id)
    {
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
    }

    public static OrderItem Create(
        Guid orderId,
        Guid productId,
        int quantity,
        Money unitPrice,
        decimal discount = 0
    )
    {
        if (orderId == Guid.Empty)
            throw new RequiredFieldException(nameof(orderId));

        if (productId == Guid.Empty)
            throw new RequiredFieldException(nameof(productId));

        if (quantity < 1)
            throw new InvalidQuantityException();

        if (discount < 0)
            throw new InvalidDiscountException(discount);

        var total = unitPrice.Amount * quantity;

        if (discount > total)
            throw new InvalidDiscountException(discount, total);

        return new OrderItem(Guid.NewGuid(), orderId, productId, quantity, unitPrice, discount);
    }
}
