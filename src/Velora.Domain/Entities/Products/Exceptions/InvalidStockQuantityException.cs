using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Products.Exceptions;

public sealed class InvalidStockQuantityException(int quantity)
    : DomainException($"Stock quantity '{quantity}' cannot be negative.");
