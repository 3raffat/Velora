using Velora.Domain.Common;
using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Domain.Entities.Orders;

public class Cancellation : BaseEntity
{
    public string Reason { get; private set; } = string.Empty;
    public CancellationStatus Status { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public Guid? ProcessedBy { get; private set; }
    public decimal OrderAmount { get; private set; }
    public decimal? CancellationCharges { get; private set; }
    public string? Remarks { get; private set; }

    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;

    public Refund? Refund { get; private set; }

    private Cancellation() { }

    private Cancellation(Guid id, string reason, decimal orderAmount, Guid orderId)
        : base(id)
    {
        Reason = reason;
        OrderAmount = orderAmount;
        OrderId = orderId;
        RequestedAt = DateTime.UtcNow;
        Status = CancellationStatus.Pending;
    }

    public static Cancellation Create(string reason, decimal orderAmount, Guid orderId)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Reason is required.", nameof(reason));

        if (orderAmount <= 0)
            throw new ArgumentOutOfRangeException(nameof(orderAmount), "Order amount must be greater than zero.");

        if (orderId == Guid.Empty)
            throw new ArgumentException("Order Id is required.", nameof(orderId));

        return new Cancellation(Guid.NewGuid(), reason.Trim(), orderAmount, orderId);
    }

    public void Approve(Guid processedBy, decimal? cancellationCharges = null)
    {
        if (Status != CancellationStatus.Pending)
            throw new InvalidOperationException("Only a pending cancellation can be approved.");

        if (cancellationCharges is < 0)
            throw new ArgumentOutOfRangeException(nameof(cancellationCharges), "Cancellation charges cannot be negative.");

        if (cancellationCharges > OrderAmount)
            throw new ArgumentOutOfRangeException(nameof(cancellationCharges), "Cancellation charges cannot exceed the order amount.");

        ProcessedBy = processedBy;
        ProcessedAt = DateTime.UtcNow;
        CancellationCharges = cancellationCharges;
        Status = CancellationStatus.Approved;
    }

    public void Reject(Guid processedBy, string remarks)
    {
        if (Status != CancellationStatus.Pending)
            throw new InvalidOperationException("Only a pending cancellation can be rejected.");

        if (string.IsNullOrWhiteSpace(remarks))
            throw new ArgumentException("Remarks are required when rejecting.", nameof(remarks));

        ProcessedBy = processedBy;
        ProcessedAt = DateTime.UtcNow;
        Remarks = remarks.Trim();
        Status = CancellationStatus.Rejected;
    }

    public void AttachRefund(Refund refund)
    {
        ArgumentNullException.ThrowIfNull(refund);

        if (Status != CancellationStatus.Approved)
            throw new InvalidOperationException("Refund can only be attached to an approved cancellation.");

        Refund = refund;
    }
}