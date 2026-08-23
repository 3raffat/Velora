using MediatR;

namespace OrderService.Application.Features.Orders.Commands.Deliver;

public sealed record DeliverOrderCommand(Guid OrderId) : IRequest;
