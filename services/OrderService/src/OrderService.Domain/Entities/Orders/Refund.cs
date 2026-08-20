using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;
using OrderService.Domain.Common.ValueObjects;
using OrderService.Domain.Entities.Orders.Enums;

namespace OrderService.Domain.Entities.Orders;

public sealed class Refund : AuditableEntity
{
    public Money Amount { get; private set; } = null!;
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
        Money amount,
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
        Money amount,
        PaymentMethod refundMethod,
        string? refundReason,
        Guid paymentId,
        Guid cancellationId
    )
    {
        if (paymentId == Guid.Empty)
            throw new RequiredFieldException(nameof(paymentId));

        if (cancellationId == Guid.Empty)
            throw new RequiredFieldException(nameof(cancellationId));

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
            throw new InvalidStatusException(
                nameof(Refund),
                nameof(Approve),
                Status,
                RefundStatus.Pending
            );

        Status = RefundStatus.Approved;
    }

    public void Complete(Guid processedBy, string transactionId)
    {
        if (Status != RefundStatus.Approved)
            throw new InvalidStatusException(
                nameof(Refund),
                nameof(Complete),
                Status,
                RefundStatus.Approved
            );

        if (string.IsNullOrWhiteSpace(transactionId))
            throw new RequiredFieldException(nameof(transactionId));

        ProcessedBy = processedBy;
        TransactionId = transactionId;
        Status = RefundStatus.Completed;
    }

    public void Reject(string reason)
    {
        if (Status != RefundStatus.Pending)
            throw new InvalidStatusException(
                nameof(Refund),
                nameof(Reject),
                Status,
                RefundStatus.Pending
            );

        if (string.IsNullOrWhiteSpace(reason))
            throw new RequiredFieldException(nameof(reason));

        RefundReason = reason.Trim();
        Status = RefundStatus.Rejected;
    }
}
