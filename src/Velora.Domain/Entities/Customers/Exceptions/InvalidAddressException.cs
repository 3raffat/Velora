using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Customers.Exceptions;

public sealed class InvalidAddressException : DomainException
{
    public InvalidAddressException(string message)
        : base(message) { }
}
