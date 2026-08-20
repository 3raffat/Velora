using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.Categories.Exceptions;

public sealed class CategoryNotFoundException(Guid id)
    : NotFoundException($"Category with ID '{id}' was not found.");
