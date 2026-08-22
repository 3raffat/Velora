namespace OrderService.Api.Contracts;

using OrderService.Application.Features.Products.Commands.UpdateStock;

public sealed record CreateProductRequest(
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    string? ImageUrl,
    Guid CategoryId
);

public sealed record UpdateProductRequest(
    string Name,
    string Description,
    decimal Price,
    string? ImageUrl,
    Guid CategoryId
);

public sealed record UpdateProductPriceRequest(decimal Price);

public sealed record UpdateProductStockRequest(int Quantity, StockOperation Operation);
