using MediatR;

namespace Velora.Application.Features.Products.Commands.UpdatePrice;

public sealed record UpdatePriceCommand(Guid Id, decimal Price) : IRequest;
