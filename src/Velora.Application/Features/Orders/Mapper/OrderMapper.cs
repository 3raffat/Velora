using Velora.Application.Features.Orders.Dtos;
using Velora.Domain.Entities.Orders;
using Velora.Domain.Entities.Orders.ValueObjects;

namespace Velora.Application.Features.Orders.Mapper;

public static class OrderMapper
{
    public static OrderSummaryDto ToSummaryDto(this Order order)
    {
        return new OrderSummaryDto(
            order.Id,
            order.OrderNumber,
            order.OrderDate,
            order.OrderStatus,
            order.TotalAmount,
            order.ShippingCost.Amount,
            order.OrderItems.Count
        );
    }

    public static OrderDetailDto ToDetailDto(this Order order)
    {
        return new OrderDetailDto(
            order.Id,
            order.OrderNumber,
            order.OrderDate,
            order.OrderStatus,
            order.TotalBaseAmount,
            order.TotalDiscountAmount,
            order.ShippingCost.Amount,
            order.TotalAmount,
            order.BillingAddress.ToDto(),
            order.ShippingAddress.ToDto(),
            order.Payment?.ToDto(),
            order.OrderItems.Select(i => i.ToDto()).ToList()
        );
    }

    public static OrderItemDto ToDto(this OrderItem item)
    {
        return new OrderItemDto(
            item.Id,
            item.ProductId,
            item.Quantity,
            item.UnitPrice.Amount,
            item.Discount,
            item.TotalPrice
        );
    }

    public static AddressSnapshotDto ToDto(this AddressSnapshot address)
    {
        return new AddressSnapshotDto(
            address.AddressLine1,
            address.AddressLine2,
            address.City,
            address.State,
            address.Country
        );
    }

    public static PaymentDto ToDto(this Payment payment)
    {
        return new PaymentDto(
            payment.Id,
            payment.PaymentMethod,
            payment.Status,
            payment.Amount.Amount,
            payment.TransactionId,
            payment.PaymentDate
        );
    }
}
