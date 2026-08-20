using MediatR;

namespace OrderService.Application.Features.Categories.Commands.Update;

public sealed record UpdateCategoryCommand(Guid Id, string Name, string Description) : IRequest;
