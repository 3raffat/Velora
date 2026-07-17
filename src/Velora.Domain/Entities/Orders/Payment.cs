using Velora.Domain.Common;
using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Domain.Entities.Orders;

public sealed class Payment : BaseEntity
{
    public PaymentMethod PaymentMethod { get; private set; }
    public string? TransactionId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public PaymentStatus Status { get; private set; }

    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;

    public Refund? Refund { get; private set; }

    private Payment() { }

    private Payment(Guid id, PaymentMethod paymentMethod, decimal amount, Guid orderId)
        : base(id)
    {
        PaymentMethod = paymentMethod;
        Amount = amount;
        OrderId = orderId;
        PaymentDate = DateTime.UtcNow;
        Status = PaymentStatus.Pending;
    }

    public static Payment Create(PaymentMethod paymentMethod, decimal amount, Guid orderId)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Amount must be greater than zero."
            );

        if (orderId == Guid.Empty)
            throw new ArgumentException("Order Id is required.", nameof(orderId));

        return new Payment(Guid.NewGuid(), paymentMethod, amount, orderId);
    }

    public void ConfirmTransaction(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
            throw new ArgumentException("Transaction Id is required.", nameof(transactionId));

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
