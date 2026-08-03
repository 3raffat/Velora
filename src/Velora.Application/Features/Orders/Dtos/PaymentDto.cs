using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Application.Features.Orders.Dtos;

public sealed record PaymentDto(
    Guid Id,
    PaymentMethod PaymentMethod,
    PaymentStatus Status,
    decimal Amount,
    string? TransactionId,
    DateTime PaymentDate
);
