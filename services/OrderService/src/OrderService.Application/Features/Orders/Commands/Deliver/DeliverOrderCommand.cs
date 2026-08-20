using MediatR;

namespace OrderService.Application.Features.Orders.Commands.Deliver;

public sealed record DeliverOrderCommand(Guid CustomerId, Guid OrderId) : IRequest;
