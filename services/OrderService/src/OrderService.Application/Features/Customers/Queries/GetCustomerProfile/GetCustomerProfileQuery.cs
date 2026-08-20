using MediatR;
using OrderService.Application.Features.Customers.Dtos;

namespace OrderService.Application.Features.Customers.Queries.GetCustomerProfile;

public sealed record GetCustomerProfileQuery(Guid IdentityUserId) : IRequest<CustomerDto>;
