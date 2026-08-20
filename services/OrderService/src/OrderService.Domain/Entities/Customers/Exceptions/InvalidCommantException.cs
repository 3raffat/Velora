using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Customers.Exceptions;

public sealed class InvalidCommentException : DomainException
{
    public InvalidCommentException()
        : base("Comment is required.") { }

    public InvalidCommentException(int maxLength)
        : base($"Comment cannot exceed {maxLength} characters.") { }

    public InvalidCommentException(int minLength, bool isMinLength)
        : base($"Comment must be at least {minLength} characters.") { }
}
