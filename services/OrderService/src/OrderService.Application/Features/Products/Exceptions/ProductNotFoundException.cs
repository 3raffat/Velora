using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.Products.Exceptions;

public sealed class ProductNotFoundException(Guid id)
    : NotFoundException($"Product with Id {id} was not found.");
