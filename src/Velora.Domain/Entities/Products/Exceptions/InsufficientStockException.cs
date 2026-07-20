using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Products.Exceptions;

public class InsufficientStockException : DomainException
{
    public InsufficientStockException(int availableQuantity, int requestedQuantity)
        : base(
            $"Requested quantity ({requestedQuantity}) exceeds available stock ({availableQuantity})."
        ) { }
}
