using MediatR;

namespace OrderService.Application.Features.Orders.Commands.RejectCancellation;

public sealed record RejectCancellationCommand(Guid ProcessedBy, Guid OrderId, string Remarks)
    : IRequest;
