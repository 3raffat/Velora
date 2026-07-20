using Velora.Application.Features.Products.Dtos;
using Velora.Domain.Entities.Products;

namespace Velora.Application.Features.Products.Mapper;

public static class ProductMapper
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(
            product.Name.Value,
            product.Description,
            product.Price.Amount,
            product.StockQuantity,
            product.ImageUrl,
            product.Category.Name.Value
        );
    }

    public static IEnumerable<ProductDto> ToDtos(this IEnumerable<Product> products)
    {
        return products.Select(p => p.ToDto());
    }
}
