using MediatR;
using Velora.Application.Features.Customers.Dtos;

namespace Velora.Application.Features.Customers.Queries.GetCustomerById;

public sealed record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto>;
