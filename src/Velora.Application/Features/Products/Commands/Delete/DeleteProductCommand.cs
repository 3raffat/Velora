using MediatR;

namespace Velora.Application.Features.Products.Commands.Delete;

public sealed record DeleteProductCommand(Guid Id) : IRequest;
