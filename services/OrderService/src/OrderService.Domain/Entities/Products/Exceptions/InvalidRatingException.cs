using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Products.Exceptions;

public sealed class InvalidRatingException : DomainException
{
    public InvalidRatingException(byte value)
        : base($"Rating '{value}' is invalid. Rating must be between 1 and 5.") { }
}
