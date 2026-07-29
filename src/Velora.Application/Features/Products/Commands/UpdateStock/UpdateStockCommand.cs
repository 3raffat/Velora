using MediatR;

namespace Velora.Application.Features.Products.Commands.UpdateStock;

public enum StockOperation
{
    Increase,
    Decrease,
}

public sealed record UpdateStockCommand(Guid Id, int Quantity, StockOperation Operation) : IRequest;
