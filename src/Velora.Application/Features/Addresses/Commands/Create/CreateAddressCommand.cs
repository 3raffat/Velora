using MediatR;

namespace Velora.Application.Features.Addresses.Commands.Create;

public sealed record CreateAddressCommand(
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string Country,
    Guid CustomerId
) : IRequest;
