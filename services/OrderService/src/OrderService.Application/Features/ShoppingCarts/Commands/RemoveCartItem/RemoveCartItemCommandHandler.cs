using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.ShoppingCarts.Exceptions;
using OrderService.Domain.Entities.ShoppingCart.Enums;

namespace OrderService.Application.Features.ShoppingCarts.Commands.RemoveCartItem;

public sealed class RemoveCartItemCommandHandler(
    IVeloraContext _context,
    ILogger<RemoveCartItemCommandHandler> _logger
) : IRequestHandler<RemoveCartItemCommand>
{
    public async Task Handle(RemoveCartItemCommand request, CancellationToken ct)
    {
        var cart = await _context
            .Carts.Include(ci => ci.CartItems)
            .FirstOrDefaultAsync(
                c =>
                    c.Id == request.CartId
                    && c.CustomerId == request.CustomerId
                    && c.Status == CartStatus.Active,
                ct
            );

        if (cart is null)
            throw new CartNotFoundException(request.CartId);

        cart.RemoveItem(request.ProductId);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Product {ProductId} removed successfully from cart {CartId}",
            request.ProductId,
            cart.Id
        );
    }
}
