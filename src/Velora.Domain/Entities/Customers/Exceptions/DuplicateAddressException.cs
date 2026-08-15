using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Customers.Exceptions;

public sealed class DuplicateAddressException : DomainException
{
    public DuplicateAddressException()
        : base("This address already exists for the customer.") { }
}
