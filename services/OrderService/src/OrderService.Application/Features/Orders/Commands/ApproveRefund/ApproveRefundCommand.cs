using MediatR;

namespace OrderService.Application.Features.Orders.Commands.ApproveRefund;

public sealed record ApproveRefundCommand(Guid ProcessedBy, Guid OrderId) : IRequest;
