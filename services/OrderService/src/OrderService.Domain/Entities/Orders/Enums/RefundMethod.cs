namespace OrderService.Domain.Entities.Orders.Enums;

public enum RefundMethod
{
    Original,
    PayPal,
    Stripe,
    BankTransfer,
    Manual,
}
