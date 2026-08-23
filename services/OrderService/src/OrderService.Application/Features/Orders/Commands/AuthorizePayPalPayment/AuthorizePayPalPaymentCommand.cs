using MediatR;

namespace OrderService.Application.Features.Orders.Commands.AuthorizePayPalPayment;

public sealed record AuthorizePayPalPaymentCommand(
    Guid CustomerId,
    Guid OrderId,
    string PayPalOrderId
) : IRequest;
