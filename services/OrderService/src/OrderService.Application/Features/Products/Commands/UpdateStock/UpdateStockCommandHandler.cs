using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Products.Exceptions;

namespace OrderService.Application.Features.Products.Commands.UpdateStock;

public sealed class UpdateStockCommandHandler(
    IVeloraContext _context,
    ILogger<UpdateStockCommandHandler> _logger
) : IRequestHandler<UpdateStockCommand>
{
    public async Task Handle(UpdateStockCommand request, CancellationToken ct)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (product is null)
            throw new ProductNotFoundException(request.Id);

        var previousQuantity = product.StockQuantity;

        switch (request.Operation)
        {
            case StockOperation.Increase:
                product.IncreaseStock(request.Quantity);
                break;
            case StockOperation.Decrease:
                product.DecreaseStock(request.Quantity);
                break;
        }

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Stock updated successfully. ProductId: {ProductId}, Previous: {Previous}, New: {New}",
            request.Id,
            previousQuantity,
            product.StockQuantity
        );
    }
}
