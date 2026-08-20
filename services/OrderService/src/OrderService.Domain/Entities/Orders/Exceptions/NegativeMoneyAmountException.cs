using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;

namespace OrderService.Domain.Entities.Orders.Exceptions;

public class NegativeMoneyAmountException(decimal amount)
    : DomainException($"Money amount cannot be negative. Amount: {amount}.");
