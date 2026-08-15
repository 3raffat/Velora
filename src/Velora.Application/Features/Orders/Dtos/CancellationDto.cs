using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Application.Features.Orders.Dtos;

public sealed record CancellationDto(
    Guid Id,
    string Reason,
    CancellationStatus Status,
    DateTime RequestedAt,
    DateTime? ProcessedAt,
    decimal OrderAmount,
    decimal? CancellationCharges,
    string? Remarks,
    RefundDto? Refund
);
