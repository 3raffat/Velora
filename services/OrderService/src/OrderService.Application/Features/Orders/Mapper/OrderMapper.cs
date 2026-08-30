using OrderService.Application.Features.Orders.Dtos;
using OrderService.Domain.Entities.Orders;
using OrderService.Domain.Entities.Orders.ValueObjects;

namespace OrderService.Application.Features.Orders.Mapper;

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
            order.OrderItems.Select(i => i.ToDto()).ToList(),
            order.Payment?.ToDto(),
            order.Cancellation?.ToDto()
        );
    }

    public static OrderItemDto ToDto(this OrderItem item)
    {
        return new OrderItemDto(
            item.Id,
            item.ProductId,
            item.Product.Name.Value,
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

    public static CancellationDto ToDto(this Cancellation cancellation)
    {
        return new CancellationDto(
            cancellation.Id,
            cancellation.OrderId,
            cancellation.Reason,
            cancellation.Status,
            cancellation.RequestedAt,
            cancellation.ProcessedAt,
            cancellation.OrderAmount.Amount,
            cancellation.CancellationCharges,
            cancellation.Remarks,
            cancellation.Refund?.ToDto(),
            cancellation.Order?.OrderNumber
        );
    }

    public static RefundDto ToDto(this Refund refund)
    {
        return new RefundDto(
            refund.Id,
            refund.Amount.Amount,
            refund.Status,
            refund.RefundMethod,
            refund.RefundReason,
            refund.TransactionId
        );
    }
}
