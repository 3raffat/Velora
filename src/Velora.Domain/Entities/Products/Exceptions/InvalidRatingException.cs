using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Products.Exceptions;

public sealed class InvalidRatingException : DomainException
{
    public InvalidRatingException(byte value)
        : base($"Rating '{value}' is invalid. Rating must be between 1 and 5.") { }
}
