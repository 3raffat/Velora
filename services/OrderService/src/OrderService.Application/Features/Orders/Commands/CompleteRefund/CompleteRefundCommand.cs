using MediatR;

namespace OrderService.Application.Features.Orders.Commands.CompleteRefund;

public sealed record CompleteRefundCommand(Guid ProcessedBy, Guid OrderId, string? TransactionId = null)
    : IRequest;
