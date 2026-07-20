using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Categories.Exceptions;

public sealed class CategoryNotFoundException(Guid id)
    : NotFoundException($"Category with ID '{id}' was not found.");
