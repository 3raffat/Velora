using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Customers.Exceptions;

public sealed class InvalidPhoneNumberException : DomainException
{
    public InvalidPhoneNumberException(string phoneNumber)
        : base($"{phoneNumber} is not a valid phone number.") { }
}
