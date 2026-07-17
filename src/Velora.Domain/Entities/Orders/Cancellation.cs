using Velora.Domain.Common;
using Velora.Domain.Entities.Orders.Enums;

namespace Velora.Domain.Entities.Orders;

public class Cancellation : BaseEntity
{
    public string Reason { get; set; } = string.Empty;
    public CancellationStatus Status { get; set; }
    public DateTime RequestedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public Guid? ProcessedBy { get; set; }
    public decimal OrderAmount { get; set; }
    public decimal? CancellationCharges { get; set; }
    public string? Remarks { get; set; }

    public Guid OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public Refund Refund { get; set; } = null!;
}
