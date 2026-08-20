using MediatR;
using OrderService.Application.Features.Orders.Dtos;

namespace OrderService.Application.Features.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid CustomerId, Guid OrderId) : IRequest<OrderDetailDto>;
