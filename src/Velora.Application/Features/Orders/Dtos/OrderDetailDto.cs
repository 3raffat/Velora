using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Application.Features.Orders.Dtos;

public sealed record OrderDetailDto(
    Guid Id,
    string OrderNumber,
    DateTime OrderDate,
    OrderStatus Status,
    decimal TotalBaseAmount,
    decimal TotalDiscountAmount,
    decimal ShippingCost,
    decimal TotalAmount,
    AddressSnapshotDto BillingAddress,
    AddressSnapshotDto ShippingAddress,
    IReadOnlyCollection<OrderItemDto> Items,
    PaymentDto? Payment,
    CancellationDto? Cancellation
);
