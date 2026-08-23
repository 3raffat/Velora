using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;
using OrderService.Domain.Common.ValueObjects;
using OrderService.Domain.Entities.Customers;
using OrderService.Domain.Entities.Customers.Exceptions;
using OrderService.Domain.Entities.Orders.Enums;
using OrderService.Domain.Entities.Orders.Events;
using OrderService.Domain.Entities.Orders.Exceptions;
using OrderService.Domain.Entities.Orders.ValueObjects;

namespace OrderService.Domain.Entities.Orders;

public sealed class Order : AuditableEntity
{
    public string OrderNumber { get; private set; } = string.Empty;
    public DateTime OrderDate { get; private set; }
    public Money ShippingCost { get; private set; } = null!;
    public OrderStatus OrderStatus { get; private set; }
    public decimal DiscountPercentage { get; private set; }
    public decimal TotalBaseAmount { get; private set; }
    public decimal TotalDiscountAmount { get; private set; }
    public decimal TotalAmount { get; private set; }
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
        Money shippingCost,
        decimal discountPercentage = 0
    )
        : base(id)
    {
        OrderNumber = GenerateOrderNumber(id);
        CustomerId = customerId;
        BillingAddress = billingAddress;
        ShippingAddress = shippingAddress;
        ShippingCost = shippingCost;
        OrderDate = DateTime.UtcNow;
        DiscountPercentage = discountPercentage;
        OrderStatus = OrderStatus.Pending;
    }

    public static Order Create(
        Guid customerId,
        AddressSnapshot? billingAddress,
        AddressSnapshot shippingAddress,
        Money shippingCost,
        PaymentMethod paymentMethod,
        decimal discountPercentage = 0
    )
    {
        if (customerId == Guid.Empty)
            throw new RequiredFieldException(nameof(customerId));

        if (paymentMethod is PaymentMethod.CashOnDelivery)
            return CreateCashOnDelivery(
                customerId,
                shippingAddress,
                shippingCost,
                discountPercentage
            );

        if (billingAddress is null)
            throw new RequiredFieldException(nameof(billingAddress));

        return CreateWithOnlinePayment(
            customerId,
            billingAddress,
            shippingAddress,
            shippingCost,
            discountPercentage
        );
    }

    private static Order CreateWithOnlinePayment(
        Guid customerId,
        AddressSnapshot billingAddress,
        AddressSnapshot shippingAddress,
        Money shippingCost,
        decimal discountPercentage = 0
    )
    {
        var order = new Order(
            Guid.NewGuid(),
            customerId,
            billingAddress,
            shippingAddress,
            shippingCost,
            discountPercentage
        );
        order.AddDomainEvent(new OrderCreatedEvent(order.Id, customerId));

        return order;
    }

    private static Order CreateCashOnDelivery(
        Guid customerId,
        AddressSnapshot shippingAddress,
        Money shippingCost,
        decimal discountPercentage = 0
    )
    {
        var order = new Order(
            Guid.NewGuid(),
            customerId,
            shippingAddress,
            shippingAddress,
            shippingCost,
            discountPercentage
        );

        order.AddDomainEvent(new OrderCreatedEvent(order.Id, customerId));

        return order;
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

        var orderItem = OrderItem.Create(this.Id, productId, quantity, unitPrice, discount);

        _orderItems.Add(orderItem);

        RecalculateTotals();
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

        OrderStatus = OrderStatus.Confirmed;
    }

    public void Ship()
    {
        if (OrderStatus != OrderStatus.Confirmed)
            throw new InvalidStatusException(
                nameof(Order),
                nameof(Ship),
                OrderStatus,
                OrderStatus.Confirmed
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

        if (Payment?.PaymentMethod == PaymentMethod.CashOnDelivery)
        {
            Payment.MarkAsCompleted();
        }
    }

    public void Cancel()
    {
        if (OrderStatus is OrderStatus.Shipped or OrderStatus.Delivered)
            throw new InvalidStatusException(
                nameof(Order),
                nameof(Cancel),
                OrderStatus,
                OrderStatus.Confirmed
            );

        if (OrderStatus == OrderStatus.Cancelled)
            throw new InvalidStatusException(
                nameof(Order),
                nameof(Cancel),
                OrderStatus,
                OrderStatus.Cancelled
            );

        OrderStatus = OrderStatus.Cancelled;
    }

    private static string GenerateOrderNumber(Guid Id)
    {
        return $"ORD-{DateTime.UtcNow.ToString("yyyyMMdd")}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    private void RecalculateTotals()
    {
        TotalBaseAmount = _orderItems.Sum(i => i.UnitPrice.Amount * i.Quantity);

        TotalDiscountAmount = TotalBaseAmount * (DiscountPercentage / 100);

        TotalAmount = TotalBaseAmount - TotalDiscountAmount + ShippingCost.Amount;
    }
}
