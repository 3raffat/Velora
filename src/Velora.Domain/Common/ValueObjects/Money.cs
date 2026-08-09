using Velora.Domain.Entities.Orders.Exceptions;

namespace Velora.Domain.Common.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; }
    public static Money Zero => new(0);

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

    public static Money operator *(Money money, int quantity) =>
        Money.Create(money.Amount * quantity);
}
