using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Customers.Exceptions;

public sealed class InvalidEmailException : DomainException
{
    public InvalidEmailException(string message)
        : base(message) { }
}
