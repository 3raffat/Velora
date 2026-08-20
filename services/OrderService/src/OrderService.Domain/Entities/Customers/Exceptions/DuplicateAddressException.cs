using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Customers.Exceptions;

public sealed class DuplicateAddressException : DomainException
{
    public DuplicateAddressException()
        : base("This address already exists for the customer.") { }
}
