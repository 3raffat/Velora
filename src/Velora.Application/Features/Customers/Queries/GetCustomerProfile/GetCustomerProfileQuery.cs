using MediatR;
using Velora.Application.Features.Customers.Dtos;

namespace Velora.Application.Features.Customers.Queries.GetCustomerProfile;

public sealed record GetCustomerProfileQuery(Guid IdentityUserId) : IRequest<CustomerDto>;
