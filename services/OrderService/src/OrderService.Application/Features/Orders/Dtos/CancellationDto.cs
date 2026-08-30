using OrderService.Domain.Entities.Orders.Enums;

namespace OrderService.Application.Features.Orders.Dtos;

public sealed record CancellationDto(
    Guid Id,
    Guid OrderId,
    string Reason,
    CancellationStatus Status,
    DateTime RequestedAt,
    DateTime? ProcessedAt,
    decimal OrderAmount,
    decimal? CancellationCharges,
    string? Remarks,
    RefundDto? Refund,
    string? OrderNumber = null
);
