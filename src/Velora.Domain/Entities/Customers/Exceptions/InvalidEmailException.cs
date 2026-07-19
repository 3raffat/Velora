using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Customers.Exceptions;

public sealed class InvalidEmailException : DomainException
{
    public InvalidEmailException(string message)
        : base(message) { }
}
