using Velora.Domain.Common;

namespace Velora.Domain.Entities.Customers.Errors;

public class InvalidNameException : DomainException
{
    public InvalidNameException(string message) : base(message)
    {
    }
}
