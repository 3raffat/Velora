using MediatR;
using Velora.Domain.Entities.Products;

namespace Velora.Application.Features.Products.Commands.Create;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string? ImageUrl,
    Guid CategoryId
) : IRequest;
