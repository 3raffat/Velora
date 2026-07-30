using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Addresses.Exceptions;

public sealed class AddressNotFoundException : NotFoundException
{
    public AddressNotFoundException(Guid addressId)
        : base($"Address with ID '{addressId}' was not found.") { }
}
