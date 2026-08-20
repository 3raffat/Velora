using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Products.Exceptions;

public sealed class InvalidStockQuantityException(int quantity)
    : DomainException($"Stock quantity '{quantity}' cannot be negative.");
