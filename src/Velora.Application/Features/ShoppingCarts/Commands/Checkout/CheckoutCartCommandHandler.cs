using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Velora.Application.Common.Interfaces;
using Velora.Application.Features.Products.Exceptions;
using Velora.Application.Features.ShoppingCarts.Exceptions;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Orders;
using Velora.Domain.Entities.Orders.ValueObjects;
using Velora.Domain.Entities.Products.Exceptions;
using Velora.Domain.Entities.ShoppingCart.Enums;

namespace Velora.Application.Features.ShoppingCarts.Commands.Checkout;

public sealed class CheckoutCartCommandHandler(
    IVeloraContext _context,
    ILogger<CheckoutCartCommandHandler> _logger
) : IRequestHandler<CheckoutCartCommand>
{
    public async Task Handle(CheckoutCartCommand request, CancellationToken ct)
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

        if (!cart.CartItems.Any())
            throw new EmptyCartException();

        var productIds = cart.CartItems.Select(c => c.ProductId);

        var products = await _context
            .Products.Where(p => productIds.Contains(p.Id))
            .ToListAsync(ct);

        //check the stock
        foreach (var item in cart.CartItems)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);

            if (product is null)
                throw new ProductNotFoundException(item.ProductId);

            if (product.StockQuantity < item.Quantity)
                throw new InsufficientStockException(product.StockQuantity, item.Quantity);
        }

        var shippingAddress = await _context.Addresses.FirstAsync(
            a => a.Id == request.ShippingAddressId && a.CustomerId == request.CustomerId,
            ct
        );

        var shippingSnapshot = AddressSnapshot.From(shippingAddress);

        var billingAddress = await _context.Addresses.FirstAsync(
            a => a.Id == request.BillingAddressId && a.CustomerId == request.CustomerId,
            ct
        );

        var billingSnapshot = AddressSnapshot.From(billingAddress);

        var order = Order.Create(
            request.CustomerId,
            billingSnapshot,
            shippingSnapshot,
            Money.Create(request.ShippingCost)
        );

        foreach (var item in cart.CartItems)
        {
            var product = products.First(p => p.Id == item.ProductId);

            order.AddItem(product.Id, item.Quantity, product.Price);

            product.DecreaseStock(item.Quantity);
        }

        cart.Checkout();

        _context.Orders.Add(order);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Order {OrderId} created for customer {CustomerId} from cart {CartId}",
            order.Id,
            request.CustomerId,
            request.CartId
        );
    }
}
