namespace OrderService.Api.Contracts;

public sealed record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string? ImageUrl,
    Guid CategoryId
);
