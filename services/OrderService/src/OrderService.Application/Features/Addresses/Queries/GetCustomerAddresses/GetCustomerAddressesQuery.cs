using MediatR;
using OrderService.Application.Features.Addresses.Dtos;

namespace OrderService.Application.Features.Addresses.Queries.GetCustomerAddresses;

public sealed record GetCustomerAddressesQuery(Guid CustomerId) : IRequest<IEnumerable<AddressDto>>;
