using MediatR;

namespace Velora.Application.Features.Categories.Commands.Update;

public sealed record UpdateCategoryCommand(Guid Id, string Name, string Description) : IRequest;
