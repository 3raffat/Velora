using MediatR;
using OrderService.Application.Features.Orders.Dtos;

namespace OrderService.Application.Features.Orders.Queries.GetAllOrders;

public sealed record GetAllOrdersQuery : IRequest<IReadOnlyCollection<OrderSummaryDto>>;
