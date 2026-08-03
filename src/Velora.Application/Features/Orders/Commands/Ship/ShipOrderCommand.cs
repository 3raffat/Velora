using MediatR;

namespace Velora.Application.Features.Orders.Commands.Ship;

public sealed record ShipOrderCommand(Guid CustomerId, Guid OrderId) : IRequest;
