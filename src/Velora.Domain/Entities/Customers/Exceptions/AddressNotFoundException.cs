using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Customers.Exceptions;

public sealed class AddressNotFoundException : DomainException
{
    public AddressNotFoundException(Guid addressId)
        : base($"Address with ID '{addressId}' was not found.") { }
}
