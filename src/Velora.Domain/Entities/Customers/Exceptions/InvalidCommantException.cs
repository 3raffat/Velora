using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Customers.Exceptions;

public sealed class InvalidCommentException : DomainException
{
    public InvalidCommentException()
        : base("Comment is required.") { }

    public InvalidCommentException(int maxLength)
        : base($"Comment cannot exceed {maxLength} characters.") { }

    public InvalidCommentException(int minLength, bool isMinLength)
        : base($"Comment must be at least {minLength} characters.") { }
}
