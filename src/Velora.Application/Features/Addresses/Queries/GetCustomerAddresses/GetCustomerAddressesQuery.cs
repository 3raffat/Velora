using MediatR;
using Velora.Application.Features.Addresses.Dtos;

namespace Velora.Application.Features.Addresses.Queries.GetCustomerAddresses;

public sealed record GetCustomerAddressesQuery(Guid CustomerId) : IRequest<IEnumerable<AddressDto>>;
