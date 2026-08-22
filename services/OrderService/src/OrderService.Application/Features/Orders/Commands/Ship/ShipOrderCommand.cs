using MediatR;

namespace OrderService.Application.Features.Orders.Commands.Ship;

public sealed record ShipOrderCommand(Guid OrderId) : IRequest;
