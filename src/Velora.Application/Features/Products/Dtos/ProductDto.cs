namespace Velora.Application.Features.Products.Dtos;

public record ProductDto(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string? ImageUrl,
    Guid CategoryId
);
