using Velora.Domain.Common;
using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Domain.Entities.Orders;

public sealed class Refund : BaseEntity
{
    public decimal Amount { get; private set; }
    public RefundStatus Status { get; private set; }
    public PaymentMethod RefundMethod { get; private set; }
    public string? RefundReason { get; private set; }
    public string? TransactionId { get; private set; }
    public Guid? ProcessedBy { get; private set; }

    public Guid PaymentId { get; private set; }
    public Payment Payment { get; private set; } = null!;

    public Guid CancellationId { get; private set; }
    public Cancellation Cancellation { get; private set; } = null!;

    private Refund() { }

    private Refund(
        Guid id,
        decimal amount,
        PaymentMethod refundMethod,
        string? refundReason,
        Guid paymentId,
        Guid cancellationId
    )
        : base(id)
    {
        Amount = amount;
        RefundMethod = refundMethod;
        RefundReason = refundReason?.Trim();
        PaymentId = paymentId;
        CancellationId = cancellationId;
        Status = RefundStatus.Pending;
    }

    public static Refund Create(
        decimal amount,
        PaymentMethod refundMethod,
        string? refundReason,
        Guid paymentId,
        Guid cancellationId
    )
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Amount must be greater than zero."
            );

        if (paymentId == Guid.Empty)
            throw new ArgumentException("Payment Id is required.", nameof(paymentId));

        if (cancellationId == Guid.Empty)
            throw new ArgumentException("Cancellation Id is required.", nameof(cancellationId));

        return new Refund(
            Guid.NewGuid(),
            amount,
            refundMethod,
            refundReason,
            paymentId,
            cancellationId
        );
    }

    public void Approve()
    {
        if (Status != RefundStatus.Pending)
            throw new InvalidOperationException("Only a pending refund can be approved.");

        Status = RefundStatus.Approved;
    }

    public void Complete(Guid processedBy, string transactionId)
    {
        if (Status != RefundStatus.Approved)
            throw new InvalidOperationException("Only an approved refund can be completed.");

        if (string.IsNullOrWhiteSpace(transactionId))
            throw new ArgumentException("Transaction Id is required.", nameof(transactionId));

        ProcessedBy = processedBy;
        TransactionId = transactionId;
        Status = RefundStatus.Completed;
    }

    public void Reject(string reason)
    {
        if (Status != RefundStatus.Pending)
            throw new InvalidOperationException("Only a pending refund can be rejected.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("A rejection reason is required.", nameof(reason));

        RefundReason = reason.Trim();
        Status = RefundStatus.Rejected;
    }
}
