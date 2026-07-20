using MediatR;
using Velora.Application.Features.Categories.Dtos;

namespace Velora.Application.Features.Categories.Commands.Create;

public sealed record CreateCategoryCommand(string Name, string Description) : IRequest<CategoryDto>;
