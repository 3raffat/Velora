using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Customers.Exceptions;

public sealed class InvalidCommantException : DomainException
{
    public InvalidCommantException()
        : base("Comment is required.") { }

    public InvalidCommantException(int maxLength)
        : base($"Comment cannot exceed {maxLength} characters.") { }

    public InvalidCommantException(int minLength, bool isMinLength)
        : base($"Comment must be at least {minLength} characters.") { }
}
