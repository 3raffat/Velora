using MediatR;

namespace Velora.Application.Features.Orders.Commands.ApproveRefund;

public sealed record ApproveRefundCommand(Guid ProcessedBy, Guid OrderId) : IRequest;
