using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Products.Exceptions;
using OrderService.Domain.Common.ValueObjects;

namespace OrderService.Application.Features.Products.Commands.UpdatePrice;

public sealed class UpdatePriceCommandHandler(
    IVeloraContext _context,
    ILogger<UpdatePriceCommandHandler> _logger
) : IRequestHandler<UpdatePriceCommand>
{
    public async Task Handle(UpdatePriceCommand request, CancellationToken ct)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, ct);

        if (product is null)
            throw new ProductNotFoundException(request.Id);

        var previousPrice = product.Price.Amount;

        product.UpdatePrice(Money.Create(request.Price));

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Price updated successfully. ProductId: {ProductId}, Previous: {Previous}, New: {New}",
            request.Id,
            previousPrice,
            product.Price.Amount
        );
    }
}
