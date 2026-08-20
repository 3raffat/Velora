using OrderService.Application.Common.Exceptions;

namespace OrderService.Application.Features.Products.Exceptions;

public sealed class ProductAlreadyExistsException : ConflictException
{
    private ProductAlreadyExistsException(string message)
        : base(message) { }

    public static ProductAlreadyExistsException ByName(string name)
    {
        return new($"Product with name {name} already exists.");
    }

    public static ProductAlreadyExistsException ById(Guid id)
    {
        return new($"Product with Id {id} already exists.");
    }
}
