using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.ShoppingCarts.Exceptions;
using Velora.Domain.Entities.ShoppingCart;
using Velora.Domain.Entities.ShoppingCart.Enums;

namespace Velora.Application.Features.ShoppingCarts.Commands.ClearCart;

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
