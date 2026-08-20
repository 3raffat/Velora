using OrderService.Domain.Common;
using OrderService.Domain.Common.Exceptions;
using OrderService.Domain.Common.ValueObjects;
using OrderService.Domain.Entities.Orders.Enums;
using OrderService.Domain.Entities.Orders.Events;
using OrderService.Domain.Entities.Orders.Exceptions;

namespace OrderService.Domain.Entities.Orders;

public class Cancellation : AuditableEntity
{
    public string Reason { get; private set; } = string.Empty;
    public CancellationStatus Status { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public Guid? ProcessedBy { get; private set; }
    public Money OrderAmount { get; private set; } = null!;
    public decimal? CancellationCharges { get; private set; }
    public string? Remarks { get; private set; }

    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;

    public Refund? Refund { get; private set; }

    private Cancellation() { }

    private Cancellation(Guid id, string reason, Money orderAmount, Guid orderId)
        : base(id)
    {
        Reason = reason;
        OrderAmount = orderAmount;
        OrderId = orderId;
        RequestedAt = DateTime.UtcNow;
        Status = CancellationStatus.Pending;
    }

    public static Cancellation Create(string reason, Money orderAmount, Guid orderId)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new RequiredFieldException(nameof(reason));

        if (orderId == Guid.Empty)
            throw new RequiredFieldException(nameof(orderId));

        return new Cancellation(Guid.NewGuid(), reason.Trim(), orderAmount, orderId);
    }

    public void Approve(Guid processedBy, decimal? cancellationCharges = null)
    {
        if (Status != CancellationStatus.Pending)
            throw new InvalidStatusException(
                nameof(Cancellation),
                nameof(Approve),
                Status,
                CancellationStatus.Pending
            );

        if (cancellationCharges is < 0)
            throw new InvalidCancellationChargesException(
                "Cancellation charges cannot be negative."
            );

        if (cancellationCharges > OrderAmount.Amount)
            throw new InvalidCancellationChargesException(
                "Cancellation charges cannot exceed the order amount."
            );

        ProcessedBy = processedBy;
        ProcessedAt = DateTime.UtcNow;
        CancellationCharges = cancellationCharges;
        Status = CancellationStatus.Approved;

        Order.AddDomainEvent(new OrderCancelledEvent(OrderId, Id, Order.CustomerId));
    }

    public void Reject(Guid processedBy, string remarks)
    {
        if (Status != CancellationStatus.Pending)
            throw new InvalidStatusException(
                nameof(Cancellation),
                nameof(Reject),
                Status,
                CancellationStatus.Pending
            );

        if (string.IsNullOrWhiteSpace(remarks))
            throw new RequiredFieldException(nameof(remarks));

        ProcessedBy = processedBy;
        ProcessedAt = DateTime.UtcNow;
        Remarks = remarks.Trim();
        Status = CancellationStatus.Rejected;
    }

    public void AttachRefund(Refund refund)
    {
        ArgumentNullException.ThrowIfNull(refund);

        if (Status != CancellationStatus.Approved)
            throw new InvalidStatusException(
                nameof(Cancellation),
                nameof(AttachRefund),
                Status,
                CancellationStatus.Approved
            );

        Refund = refund;
    }
}
