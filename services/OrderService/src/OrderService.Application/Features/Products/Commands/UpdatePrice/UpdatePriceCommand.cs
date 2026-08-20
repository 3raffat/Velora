using MediatR;

namespace OrderService.Application.Features.Products.Commands.UpdatePrice;

public sealed record UpdatePriceCommand(Guid Id, decimal Price) : IRequest;
