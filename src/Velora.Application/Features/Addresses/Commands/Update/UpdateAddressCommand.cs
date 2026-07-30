using MediatR;

namespace Velora.Application.Features.Addresses.Commands.Update;

public sealed record UpdateAddressCommand(
    string AddressLine1,
    string AddressLine2,
    string City,
    string State,
    string Country,
    Guid CustomerId,
    Guid AddressId
) : IRequest;
