using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Customers;
using Velora.Domain.Entities.Customers.Exceptions;
using Velora.Domain.Entities.Orders.Enums;
using Velora.Domain.Entities.Orders.Exceptions;
using Velora.Domain.Entities.Orders.ValueObjects;

namespace Velora.Domain.Entities.Orders;

public sealed class Order : AuditableEntity
{
    public string OrderNumber { get; private set; } = string.Empty;
    public DateTime OrderDate { get; private set; }
    public Money ShippingCost { get; private set; } = null!;
    public OrderStatus OrderStatus { get; private set; }

    public decimal TotalBaseAmount => _orderItems.Sum(i => i.UnitPrice.Amount * i.Quantity);
    public decimal TotalDiscountAmount => _orderItems.Sum(i => i.Discount);
    public decimal TotalAmount => TotalBaseAmount - TotalDiscountAmount + ShippingCost.Amount;

    public Guid CustomerId { get; private set; }
    public Customer Customer { get; private set; } = null!;

    public AddressSnapshot BillingAddress { get; private set; } = null!;

    public AddressSnapshot ShippingAddress { get; private set; } = null!;
    public Cancellation? Cancellation { get; private set; }

    public Payment? Payment { get; private set; }

    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private Order() { }

    private Order(
        Guid id,
        Guid customerId,
        AddressSnapshot billingAddress,
        AddressSnapshot shippingAddress,
        Money shippingCost
    )
        : base(id)
    {
        OrderNumber = GenerateOrderNumber(id);
        CustomerId = customerId;
        BillingAddress = billingAddress;
        ShippingAddress = shippingAddress;
        ShippingCost = shippingCost;
        OrderDate = DateTime.UtcNow;
        OrderStatus = OrderStatus.Pending;
    }

    public static Order Create(
        Guid customerId,
        AddressSnapshot billingAddress,
        AddressSnapshot shippingAddress,
        Money shippingCost
    )
    {
        if (customerId == Guid.Empty)
            throw new RequiredFieldException(nameof(customerId));

        return new Order(Guid.NewGuid(), customerId, billingAddress, shippingAddress, shippingCost);
    }

    public void AddItem(Guid productId, int quantity, Money unitPrice, decimal discount = 0)
    {
        if (OrderStatus != OrderStatus.Pending)
            throw new InvalidStatusException(
                nameof(Order),
                nameof(AddItem),
                OrderStatus,
                OrderStatus.Pending
            );

        var orderItem = OrderItem.Create(this.Id, productId, quantity, unitPrice);

        _orderItems.Add(orderItem);
    }

    public void Confirm()
    {
        if (OrderStatus != OrderStatus.Pending)
            throw new InvalidStatusException(
                nameof(Order),
                nameof(Confirm),
                OrderStatus,
                OrderStatus.Pending
            );

        OrderStatus = OrderStatus.Processing;
    }

    public void Ship()
    {
        if (OrderStatus != OrderStatus.Processing)
            throw new InvalidStatusException(
                nameof(Order),
                nameof(Ship),
                OrderStatus,
                OrderStatus.Processing
            );

        OrderStatus = OrderStatus.Shipped;
    }

    public void Deliver()
    {
        if (OrderStatus != OrderStatus.Shipped)
            throw new InvalidStatusException(
                nameof(Order),
                nameof(Deliver),
                OrderStatus,
                OrderStatus.Shipped
            );

        OrderStatus = OrderStatus.Delivered;
    }

    public void Cancel()
    {
        if (OrderStatus is OrderStatus.Shipped or OrderStatus.Delivered)
            throw new InvalidStatusException("Cannot cancel an order that has already shipped.");

        if (OrderStatus == OrderStatus.Canceled)
            throw new InvalidStatusException(
                nameof(Order),
                nameof(Cancel),
                OrderStatus,
                OrderStatus.Canceled
            );

        OrderStatus = OrderStatus.Canceled;
    }

    private static string GenerateOrderNumber(Guid Id)
    {
        return $"ORD-{DateTime.UtcNow.ToString("yyyyMMdd")}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }
}
