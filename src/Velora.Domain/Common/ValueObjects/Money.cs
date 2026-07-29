using Velora.Domain.Entities.Orders.Exceptions;

namespace Velora.Domain.Common.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }

    private Money(decimal amount)
    {
        Amount = amount;
    }

    public static Money Create(decimal amount)
    {
        if (amount < 0)
            throw new NegativeMoneyAmountException(amount);

        return new Money(amount);
    }
}
