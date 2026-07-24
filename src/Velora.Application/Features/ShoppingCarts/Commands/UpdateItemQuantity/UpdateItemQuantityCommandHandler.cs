using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.ShoppingCarts.Exceptions;
using Velora.Domain.Entities.ShoppingCart.Enums;

namespace Velora.Application.Features.ShoppingCarts.Commands.UpdateItemQuantity;

public sealed class UpdateItemQuantityCommandHandler(
    IVeloraContext _context,
    ILogger<UpdateItemQuantityCommandHandler> _logger
) : IRequestHandler<UpdateItemQuantityCommand>
{
    public async Task Handle(UpdateItemQuantityCommand request, CancellationToken ct)
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

        cart.UpdateQuantity(request.ProductId, request.Quantity);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Product {ProductId} quantity updated successfully in cart {CartId}. New quantity: {Quantity}",
            request.ProductId,
            cart.Id,
            request.Quantity
        );
    }
}
