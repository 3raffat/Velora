using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Products.Exceptions;

public sealed class ProductNotFoundException(Guid id)
    : NotFoundException($"Product with Id {id} was not found.");
