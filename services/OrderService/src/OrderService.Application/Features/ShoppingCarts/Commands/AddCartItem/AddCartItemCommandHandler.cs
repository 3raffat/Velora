using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Products.Exceptions;
using OrderService.Domain.Entities.Products.Exceptions;
using OrderService.Domain.Entities.ShoppingCart;
using OrderService.Domain.Entities.ShoppingCart.Enums;

namespace OrderService.Application.Features.ShoppingCarts.Commands.AddCartItem;

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

        if (product.StockQuantity < request.Quantity)
            throw new InsufficientStockException(product.StockQuantity, request.Quantity);

        var cart = await _context
            .Carts.Include(c => c.CartItems)
            .FirstOrDefaultAsync(
                c => c.CustomerId == request.CustomerId && c.Status == CartStatus.Active,
                ct
            );

        if (cart is null)
        {
            cart = Cart.Create(request.CustomerId);

            await _context.Carts.AddAsync(cart, ct);
        }

        cart.AddItem(request.ProductId, request.Quantity, product.Price.Amount);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Product {ProductId} added successfully to cart {CartId}",
            request.ProductId,
            cart.Id
        );
    }
}
