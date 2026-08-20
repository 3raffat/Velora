using MediatR;
using OrderService.Application.Features.Categories.Dtos;

namespace OrderService.Application.Features.Categories.Commands.Create;

public sealed record CreateCategoryCommand(string Name, string Description) : IRequest<CategoryDto>;
