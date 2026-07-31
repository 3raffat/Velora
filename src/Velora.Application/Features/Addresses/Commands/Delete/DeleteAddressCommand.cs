using MediatR;

namespace Velora.Application.Features.Addresses.Commands.Delete;

public record DeleteAddressCommand(Guid AddressId, Guid CustomerId) : IRequest;
