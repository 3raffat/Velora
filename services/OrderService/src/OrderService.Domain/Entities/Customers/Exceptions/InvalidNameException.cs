using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Customers.Exceptions;

public class InvalidNameException : DomainException
{
    public InvalidNameException(string message)
        : base(message) { }
}
