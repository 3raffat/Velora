using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Customers.Exceptions;

public sealed class AddressNotFoundException : DomainException
{
    public AddressNotFoundException(Guid addressId)
        : base($"Address with ID '{addressId}' was not found.") { }
}
