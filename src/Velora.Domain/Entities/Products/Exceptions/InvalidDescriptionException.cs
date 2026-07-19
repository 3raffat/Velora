using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Products.Exceptions;

public sealed class InvalidDescriptionException : DomainException
{
    public InvalidDescriptionException()
        : base("Description is required.") { }

    public InvalidDescriptionException(int maxLength)
        : base($"Description cannot exceed {maxLength} characters.") { }

    public InvalidDescriptionException(int minLength, bool isMinLength)
        : base($"Description must be at least {minLength} characters.") { }
}
