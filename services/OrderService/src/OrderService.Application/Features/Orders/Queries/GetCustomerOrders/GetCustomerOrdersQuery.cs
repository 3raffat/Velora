using MediatR;
using OrderService.Application.Features.Orders.Dtos;

namespace OrderService.Application.Features.Orders.Queries.GetCustomerOrders;

public sealed record GetCustomerOrdersQuery(Guid CustomerId)
    : IRequest<IReadOnlyCollection<OrderSummaryDto>>;
