using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Application.Features.Orders.Dtos;

public sealed record OrderSummaryDto(
    Guid Id,
    string OrderNumber,
    DateTime OrderDate,
    OrderStatus Status,
    decimal TotalAmount,
    decimal ShippingCost,
    int ItemCount
);
