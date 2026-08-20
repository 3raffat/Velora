using MediatR;
using OrderService.Application.Features.Products.Dtos;
using OrderService.Domain.Entities.Products;

namespace OrderService.Application.Features.Products.Commands.Create;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string? ImageUrl,
    Guid CategoryId
) : IRequest<ProductDto>;
