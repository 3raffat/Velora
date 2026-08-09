using MediatR;
using Velora.Application.Features.Categories.Dtos;

namespace Velora.Application.Features.Categories.Queries.GetAllCategories;

public sealed record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;
