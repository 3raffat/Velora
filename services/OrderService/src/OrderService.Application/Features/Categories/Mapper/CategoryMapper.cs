using OrderService.Application.Features.Categories.Dtos;
using CategoryEntity = OrderService.Domain.Entities.Products.Category;

namespace OrderService.Application.Features.Categories.Mapper;

public static class CategoryMapper
{
    public static CategoryDto ToDto(this CategoryEntity category)
    {
        return new CategoryDto(category.Id, category.Name.Value, category.Description);
    }

    public static IEnumerable<CategoryDto> ToDtos(this IEnumerable<CategoryEntity> categories)
    {
        return categories.Select(c => c.ToDto());
    }
}
