using MediatR;

namespace OrderService.Application.Features.Products.Commands.Update;

public sealed record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string? ImageUrl,
    Guid CategoryId
) : IRequest;
