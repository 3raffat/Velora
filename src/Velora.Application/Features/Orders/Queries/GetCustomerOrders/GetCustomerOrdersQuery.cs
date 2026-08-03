using MediatR;
using Velora.Application.Features.Orders.Dtos;

namespace Velora.Application.Features.Orders.Queries.GetCustomerOrders;

public sealed record GetCustomerOrdersQuery(Guid CustomerId)
    : IRequest<IReadOnlyCollection<OrderSummaryDto>>;
