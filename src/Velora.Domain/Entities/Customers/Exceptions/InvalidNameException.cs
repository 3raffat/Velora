using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Customers.Exceptions;

public class InvalidNameException : DomainException
{
    public InvalidNameException(string message)
        : base(message) { }
}
