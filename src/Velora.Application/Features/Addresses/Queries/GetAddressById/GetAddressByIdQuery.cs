using MediatR;
using Velora.Application.Features.Addresses.Dtos;

namespace Velora.Application.Features.Addresses.Queries.GetAddressById;

public sealed record GetAddressByIdQuery(Guid AddressId, Guid CustomerId) : IRequest<AddressDto>;
