using MediatR;

namespace Velora.Application.Features.Orders.Commands.RejectRefund;

public sealed record RejectRefundCommand(Guid ProcessedBy, Guid OrderId, string Reason) : IRequest;
