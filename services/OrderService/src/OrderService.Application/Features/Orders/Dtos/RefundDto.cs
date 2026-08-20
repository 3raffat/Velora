using OrderService.Domain.Entities.Orders.Enums;

namespace OrderService.Application.Features.Orders.Dtos;

public sealed record RefundDto(
    Guid Id,
    decimal Amount,
    RefundStatus Status,
    PaymentMethod RefundMethod,
    string? RefundReason,
    string? TransactionId
);
