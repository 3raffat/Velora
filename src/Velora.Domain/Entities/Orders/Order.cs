using Velora.Domain.Common;
using Velora.Domain.Entities.Customers;
using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Domain.Entities.Orders;

public sealed class Order : BaseEntity
{

    public string OrderNumber { get; private set; } = string.Empty;
    public DateTime OrderDate { get; private set; }
    public decimal ShippingCost { get; private set; }
    public OrderStatus OrderStatus { get; private set; }

    public decimal TotalBaseAmount => _orderItems.Sum(i => i.UnitPrice * i.Quantity);
    public decimal TotalDiscountAmount => _orderItems.Sum(i => i.Discount);
    public decimal TotalAmount => TotalBaseAmount - TotalDiscountAmount + ShippingCost;

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

    private Order(Guid id, string orderNumber, Guid customerId, Guid billingAddressId,
        Guid shippingAddressId, decimal shippingCost)
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

    public static Order Create(string orderNumber, Guid customerId, Guid billingAddressId,
        Guid shippingAddressId, decimal shippingCost)
    {
        if (string.IsNullOrWhiteSpace(orderNumber))
            throw new ArgumentException("Order number is required.", nameof(orderNumber));

        if (customerId == Guid.Empty)
            throw new ArgumentException("Customer Id is required.", nameof(customerId));

        if (billingAddressId == Guid.Empty)
            throw new ArgumentException("Billing address Id is required.", nameof(billingAddressId));

        if (shippingAddressId == Guid.Empty)
            throw new ArgumentException("Shipping address Id is required.", nameof(shippingAddressId));

        if (shippingCost < 0)
            throw new ArgumentOutOfRangeException(nameof(shippingCost), "Shipping cost cannot be negative.");

        return new Order(Guid.NewGuid(), orderNumber.Trim(), customerId, billingAddressId, shippingAddressId, shippingCost);
    }

    public void AddItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (OrderStatus != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot modify an order that is no longer pending.");

        _orderItems.Add(item);
    }


    public void Ship()
    {
        if (OrderStatus != OrderStatus.Processing)
            throw new InvalidOperationException("Only an order being processed can be shipped.");

        OrderStatus = OrderStatus.Shipped;
    }

    public void Deliver()
    {
        if (OrderStatus != OrderStatus.Shipped)
            throw new InvalidOperationException("Only a shipped order can be marked delivered.");

        OrderStatus = OrderStatus.Delivered;
    }

    public void Cancel()
    {
        if (OrderStatus is OrderStatus.Shipped or OrderStatus.Delivered)
            throw new InvalidOperationException("Cannot cancel an order that has already shipped.");

        if (OrderStatus == OrderStatus.Canceled)
            throw new InvalidOperationException("Order is already canceled.");

        OrderStatus = OrderStatus.Canceled;
    }
}