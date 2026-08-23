using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Features.Orders.Exceptions;
using OrderService.Domain.Entities.Orders.Enums;

namespace OrderService.Application.Features.Orders.Commands.AuthorizePayPalPayment;

public sealed class AuthorizePayPalPaymentCommandHandler(
    IVeloraContext context,
    IPayPalClient paypalClient
) : IRequestHandler<AuthorizePayPalPaymentCommand>
{
    public async Task Handle(AuthorizePayPalPaymentCommand request, CancellationToken ct)
    {
        var order = await context
            .Orders.Include(order => order.Payment)
            .FirstOrDefaultAsync(
                order => order.Id == request.OrderId && order.CustomerId == request.CustomerId,
                ct
            );

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);

        if (order.Payment is null || order.Payment.PaymentMethod != PaymentMethod.PayPal)
            throw new InvalidOperationException("The order does not use PayPal.");

        var captureId = await paypalClient.CaptureOrderAsync(request.PayPalOrderId, ct);

        order.Payment.ConfirmTransaction(captureId);

        await context.SaveChangesAsync(ct);
    }
}
