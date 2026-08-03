using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Orders.Commands.Cancel;
using Velora.Application.Features.Orders.Exceptions;
using Velora.Application.Features.Products.Exceptions;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Orders;

namespace Velora.Application.Features.Orders.Commands.Cancel;

public sealed class CancelOrderCommandHandler(
    IVeloraContext _context,
    ILogger<CancelOrderCommandHandler> _logger
) : IRequestHandler<CancelOrderCommand>
{
    public async Task Handle(CancelOrderCommand request, CancellationToken ct)
    {
        var order = await _context
            .Orders.Include(o => o.OrderItems)
            .FirstOrDefaultAsync(
                o => o.Id == request.OrderId && o.CustomerId == request.CustomerId,
                ct
            );

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);

        order.Cancel();

        var cancellation = Cancellation.Create(
            request.Reason,
            Money.Create(order.TotalAmount),
            order.Id
        );

        var productIds = order.OrderItems.Select(o => o.ProductId).ToList();

        var products = await _context
            .Products.Where(p => productIds.Contains(p.Id))
            .ToListAsync(ct);

        foreach (var item in order.OrderItems)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);

            if (product is null)
                throw new ProductNotFoundException(item.ProductId);

            product.IncreaseStock(item.Quantity);
        }

        await _context.Cancellations.AddAsync(cancellation);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Order {OrderId} canceled for customer {CustomerId}. Reason: {Reason}",
            order.Id,
            request.CustomerId,
            request.Reason
        );
    }
}
