using MediatR;

namespace OrderService.Application.Features.Products.Commands.Delete;

public sealed record DeleteProductCommand(Guid Id) : IRequest;
