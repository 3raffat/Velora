using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Addresses.Exceptions;
using OrderService.Application.Features.Products.Exceptions;
using OrderService.Application.Features.ShoppingCarts.Exceptions;
using OrderService.Domain.Common.ValueObjects;
using OrderService.Domain.Entities.Coupons;
using OrderService.Domain.Entities.Orders;
using OrderService.Domain.Entities.Orders.ValueObjects;
using OrderService.Domain.Entities.Products.Exceptions;
using OrderService.Domain.Entities.ShoppingCart.Enums;

namespace OrderService.Application.Features.ShoppingCarts.Commands.Checkout;

public sealed class CheckoutCartCommandHandler(
    IVeloraContext _context,
    ILogger<CheckoutCartCommandHandler> _logger
) : IRequestHandler<CheckoutCartCommand, Guid>
{
    public async Task<Guid> Handle(CheckoutCartCommand request, CancellationToken ct)
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

        var shippingAddress = await _context.Addresses.FirstAsync(
            a => a.Id == request.ShippingAddressId && a.CustomerId == request.CustomerId,
            ct
        );

        var shippingSnapshot = AddressSnapshot.From(shippingAddress);

        AddressSnapshot? billingSnapshot = null;

        if (request.BillingAddressId is not null)
        {
            var billingAddress = await _context.Addresses.FirstOrDefaultAsync(
                a => a.Id == request.BillingAddressId && a.CustomerId == request.CustomerId,
                ct
            );

            if (billingAddress is null)
                throw new AddressNotFoundException(request.BillingAddressId);

            billingSnapshot = AddressSnapshot.From(billingAddress);
        }

        var coupon = await _context.Coupons.FirstOrDefaultAsync(
            c => c.Code == request.PromoCode && c.CustomerId == request.CustomerId,
            ct
        );
        var discountPercentage = coupon?.Discount.Amount ?? 0;

        var order = Order.Create(
            request.CustomerId,
            billingSnapshot,
            shippingSnapshot,
            Money.Create(request.ShippingCost),
            request.PaymentMethod,
            discountPercentage
        );

        foreach (var item in cart.CartItems)
        {
            var product =
                products.FirstOrDefault(p => p.Id == item.ProductId)
                ?? throw new ProductNotFoundException(item.ProductId);

            if (product.StockQuantity < item.Quantity)
                throw new InsufficientStockException(product.StockQuantity, item.Quantity);

            order.AddItem(product.Id, item.Quantity, product.Price);
            product.DecreaseStock(item.Quantity);
        }

        // Create a pending payment for the order
        var payment = Payment.Create(
            request.PaymentMethod,
            Money.Create(order.TotalAmount),
            order.Id
        );

        cart.Checkout();
        coupon?.Use();
        order.Confirm();

        await _context.Orders.AddAsync(order);
        await _context.Payments.AddAsync(payment);

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Order {OrderId} ({OrderNumber}) created with payment {PaymentId} for customer {CustomerId} from cart {CartId}",
            order.Id,
            order.OrderNumber,
            payment.Id,
            request.CustomerId,
            request.CartId
        );

        return order.Id;
    }
}
