using MediatR;
using OrderService.Application.Features.Categories.Dtos;

namespace OrderService.Application.Features.Categories.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;
