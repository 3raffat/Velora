using Velora.Domain.Common;
using Velora.Domain.Entities.Customers;
using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Domain.Entities.Orders;

public sealed class Order : BaseEntity
{
    public string OrderNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TolalBaseAmount { get; set; }
    public decimal TotlaDiscountAmount { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal TotalAmount { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public Guid BillingAddressId { get; set; }
    public Address BillingAddress { get; set; } = null!;

    public Guid ShippingAddressId { get; set; }
    public Address ShippingAddress { get; set; } = null!;

    public Cancellation Cancellation { get; set; } = null!;

    public Guid PaymentId { get; set; }
    public Payment Payment { get; set; } = null!;

    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
}
