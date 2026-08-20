using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Customers.Exceptions;

public class InvalidDateOfBirthException : DomainException
{
    public InvalidDateOfBirthException()
        : base("Birth date is required.") { }

    public InvalidDateOfBirthException(DateOnly dateOfBirth)
        : base($"Birth date '{dateOfBirth:yyyy-MM-dd}' cannot be in the future.") { }
}
