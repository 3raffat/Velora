using OrderService.Application.Features.Products.Dtos;
using OrderService.Domain.Entities.Products;

namespace OrderService.Application.Features.Products.Mapper;

public static class ProductMapper
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(
            product.Id,
            product.Name.Value,
            product.Description,
            product.Price.Amount,
            product.StockQuantity,
            product.ImageUrl,
            product.IsAvailable,
            product.CategoryId
        );
    }

    public static IEnumerable<ProductDto> ToDtos(this IEnumerable<Product> products)
    {
        return products.Select(p => p.ToDto());
    }
}
