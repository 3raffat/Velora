using Velora.Domain.Common;

namespace Velora.Domain.Entities.Customers.Errors;

public sealed class InvalidEmailException : DomainException
{
    public InvalidEmailException(string message) : base(message)
    {
    }
}
