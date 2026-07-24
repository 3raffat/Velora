using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Products.Exceptions;
using Velora.Domain.Entities.ShoppingCart;
using Velora.Domain.Entities.ShoppingCart.Enums;

namespace Velora.Application.Features.ShoppingCarts.Commands.AddCartItem;

public sealed class AddCartItemCommandHandler(
    IVeloraContext _context,
    ILogger<AddCartItemCommandHandler> _logger
) : IRequestHandler<AddCartItemCommand>
{
    public async Task Handle(AddCartItemCommand request, CancellationToken ct)
    {
        var product = await _context.Products.FirstOrDefaultAsync(
            p => p.Id == request.ProductId,
            ct
        );

        if (product is null)
            throw new ProductNotFoundException(request.ProductId);

        var cart = await _context
            .Carts.Include(ci => ci.CartItems)
            .FirstOrDefaultAsync(
                c => c.CustomerId == request.CustomerId && c.Status == CartStatus.Active,
                ct
            );

        Console.WriteLine($"context Hash {_context.GetHashCode()}");

        if (cart is null)
        {
            cart = Cart.Create(request.CustomerId);

            await _context.Carts.AddAsync(cart, ct);
        }

        cart.AddItem(request.ProductId, request.Quantity, product.Price.Amount);

        foreach (var entry in _context.GetTrackedEntries().Where(x => x.Entity is CartItem))
        {
            var item = (CartItem)entry.Entity;

            Console.WriteLine(
                $"| CartId: {item.CartId}| ProductId: {item.ProductId} | State: {entry.State}"
            );
        }

        Console.WriteLine($"context Hash {_context.GetHashCode()}");

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Product {ProductId} added successfully to cart {CartId}",
            request.ProductId,
            cart.Id
        );
    }
}
