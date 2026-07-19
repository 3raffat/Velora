using Velora.Domain.Common;
using Velora.Domain.Common.Exceptions;
using Velora.Domain.Common.ValueObjects;
using Velora.Domain.Entities.Orders.Enums;
using Velora.Domain.Entities.Orders.Exceptions;

namespace Velora.Domain.Entities.Orders;

public sealed class Payment : BaseEntity
{
    public PaymentMethod PaymentMethod { get; private set; }
    public string? TransactionId { get; private set; }
    public Money Amount { get; private set; } = null!;
    public DateTime PaymentDate { get; private set; }
    public PaymentStatus Status { get; private set; }

    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;

    public Refund? Refund { get; private set; }

    private Payment() { }

    private Payment(Guid id, PaymentMethod paymentMethod, Money amount, Guid orderId)
        : base(id)
    {
        PaymentMethod = paymentMethod;
        Amount = amount;
        OrderId = orderId;
        PaymentDate = DateTime.UtcNow;
        Status = PaymentStatus.Pending;
    }

    public static Payment Create(PaymentMethod paymentMethod, Money amount, Guid orderId)
    {
        if (orderId == Guid.Empty)
            throw new RequiredFieldException(nameof(orderId));

        return new Payment(Guid.NewGuid(), paymentMethod, amount, orderId);
    }

    public void ConfirmTransaction(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
            throw new RequiredFieldException(nameof(transactionId));

        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only a pending payment can be confirmed.");

        TransactionId = transactionId;
        Status = PaymentStatus.Completed;
    }

    public void MarkFailed()
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only a pending payment can be marked as failed.");

        Status = PaymentStatus.Failed;
    }
}
