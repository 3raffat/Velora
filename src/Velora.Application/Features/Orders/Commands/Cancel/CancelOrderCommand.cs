using MediatR;

namespace Velora.Application.Features.Orders.Commands.Cancel;

public sealed record CancelOrderCommand(Guid CustomerId, Guid OrderId, string Reason) : IRequest;
