using MediatR;
using OrderService.Application.Features.Customers.Dtos;

namespace OrderService.Application.Features.Customers.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto>;
