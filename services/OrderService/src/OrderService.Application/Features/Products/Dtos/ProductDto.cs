namespace OrderService.Application.Features.Products.Dtos;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string? ImageUrl,
    bool IsAvailable,
    Guid CategoryId
);
