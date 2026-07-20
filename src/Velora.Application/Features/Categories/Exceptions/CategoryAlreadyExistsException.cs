using Velora.Application.Common.Exceptions;

namespace Velora.Application.Features.Categories.Exceptions;

public sealed class CategoryAlreadyExistsException : ConflictException
{
    private CategoryAlreadyExistsException(string message)
        : base(message) { }

    public static CategoryAlreadyExistsException ByName(string name)
    {
        return new($"Category with name '{name}' already exists.");
    }

    public static CategoryAlreadyExistsException ById(Guid id)
    {
        return new($"Category with Id '{id}' already exists.");
    }
}
