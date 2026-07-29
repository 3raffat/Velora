using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;

namespace Velora.Domain.Entities.Orders.Exceptions;

public class NegativeMoneyAmountException(decimal amount)
    : DomainException($"Money amount cannot be negative. Amount: {amount}.");
