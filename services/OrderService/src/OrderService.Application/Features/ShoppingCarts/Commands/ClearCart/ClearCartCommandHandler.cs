using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.ShoppingCarts.Exceptions;
using OrderService.Domain.Entities.ShoppingCart;
using OrderService.Domain.Entities.ShoppingCart.Enums;

namespace OrderService.Application.Features.ShoppingCarts.Commands.ClearCart;

public class ClearCartCommandHandler(
    IVeloraContext _context,
    ILogger<ClearCartCommandHandler> _logger
) : IRequestHandler<ClearCartCommand>
{
    public async Task Handle(ClearCartCommand request, CancellationToken ct)
    {
        var cart = await _context
            .Carts.Include(c => c.CartItems)
            .FirstOrDefaultAsync(
                c =>
                    c.Id == request.CartId
                    && c.CustomerId == request.CustomerId
                    && c.Status == CartStatus.Active,
                ct
            );

        if (cart is null)
            throw new CartNotFoundException(request.CartId);

        cart.Clear();

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Cart {CartId} cleared successfully for customer {CustomerId}.",
            cart.Id,
            cart.CustomerId
        );
    }
}
