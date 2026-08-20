using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Products.Exceptions;

public sealed class InvalidDescriptionException : DomainException
{
    public InvalidDescriptionException()
        : base("Description is required.") { }

    public InvalidDescriptionException(int maxLength)
        : base($"Description cannot exceed {maxLength} characters.") { }

    public InvalidDescriptionException(int minLength, bool isMinLength)
        : base($"Description must be at least {minLength} characters.") { }
}
