using MediatR;

namespace Velora.Application.Features.Orders.Commands.RequestCancellation;

public sealed record RequestCancellationCommand(Guid OrderId, Guid CustomerId, string Reason)
    : IRequest;
