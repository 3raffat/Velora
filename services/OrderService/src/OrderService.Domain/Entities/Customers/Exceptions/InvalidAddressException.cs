using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Customers.Exceptions;

public sealed class InvalidAddressException : DomainException
{
    public InvalidAddressException(string message)
        : base(message) { }
}
