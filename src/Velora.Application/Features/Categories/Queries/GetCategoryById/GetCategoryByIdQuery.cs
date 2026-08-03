using MediatR;
using Velora.Application.Features.Categories.Dtos;

namespace Velora.Application.Features.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto>;
