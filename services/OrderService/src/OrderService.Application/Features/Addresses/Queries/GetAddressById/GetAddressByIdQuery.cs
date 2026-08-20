using MediatR;
using OrderService.Application.Features.Addresses.Dtos;

namespace OrderService.Application.Features.Addresses.Queries.GetAddressById;

public sealed record GetAddressByIdQuery(Guid AddressId, Guid CustomerId) : IRequest<AddressDto>;
