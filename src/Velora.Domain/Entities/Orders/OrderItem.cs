using Velora.Domain.Common;
using Velora.Domain.Entities.Products;

namespace Velora.Domain.Entities.Orders;

public sealed class OrderItem : BaseEntity
{
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalPrice => (UnitPrice * Quantity) - Discount;

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
        decimal unitPrice,
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
        decimal unitPrice,
        decimal discount = 0
    )
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("Order Id is required.", nameof(orderId));

        if (productId == Guid.Empty)
            throw new ArgumentException("Product Id is required.", nameof(productId));

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

        return new OrderItem(Guid.NewGuid(), orderId, productId, quantity, unitPrice, discount);
    }
}
