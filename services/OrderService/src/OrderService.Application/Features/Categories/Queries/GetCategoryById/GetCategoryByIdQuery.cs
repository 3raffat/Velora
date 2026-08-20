using MediatR;
using OrderService.Application.Features.Categories.Dtos;

namespace OrderService.Application.Features.Categories.Queries.GetCategoryById;

public sealed record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto>;
