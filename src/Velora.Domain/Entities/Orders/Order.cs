using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Customers;
using Velora.Domain.Entities.Customers.Exceptions;
using Velora.Domain.Entities.Orders.Enums;
using Velora.Domain.Entities.Orders.Exceptions;

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

    public Guid BillingAddressId { get; private set; }
    public Address BillingAddress { get; private set; } = null!;

    public Guid ShippingAddressId { get; private set; }
    public Address ShippingAddress { get; private set; } = null!;

    public Cancellation? Cancellation { get; private set; }

    public Payment? Payment { get; private set; }

    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();

    private Order() { }

    private Order(
        Guid id,
        string orderNumber,
        Guid customerId,
        Guid billingAddressId,
        Guid shippingAddressId,
        Money shippingCost
    )
        : base(id)
    {
        OrderNumber = orderNumber;
        CustomerId = customerId;
        BillingAddressId = billingAddressId;
        ShippingAddressId = shippingAddressId;
        ShippingCost = shippingCost;
        OrderDate = DateTime.UtcNow;
        OrderStatus = OrderStatus.Pending;
    }

    public static Order Create(
        string orderNumber,
        Guid customerId,
        Guid billingAddressId,
        Guid shippingAddressId,
        Money shippingCost
    )
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new RequiredFieldException(nameof(orderNumber));

        if (customerId == Guid.Empty)
            throw new RequiredFieldException(nameof(customerId));

        if (billingAddressId == Guid.Empty)
            throw new RequiredFieldException(nameof(billingAddressId));

        if (shippingAddressId == Guid.Empty)
            throw new RequiredFieldException(nameof(shippingAddressId));

        return new Order(
            Guid.NewGuid(),
            orderNumber.Trim(),
            customerId,
            billingAddressId,
            shippingAddressId,
            shippingCost
        );
    }

    public void AddItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (OrderStatus != OrderStatus.Pending)
            throw new InvalidStatusException(
                nameof(Order),
                nameof(AddItem),
                OrderStatus,
                OrderStatus.Pending
            );

        _orderItems.Add(item);
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
}
