using MediatR;

namespace Velora.Application.Features.Orders.Commands.CompleteRefund;

public sealed record CompleteRefundCommand(Guid ProcessedBy, Guid OrderId, string TransactionId)
    : IRequest;
