namespace DeliveryService.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; private set; }
    public Guid? CreatedBy { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public Guid? UpdatedBy { get; private set; }

    protected AuditableEntity() { }

    protected AuditableEntity(Guid id)
        : base(id) { }

    public void MarkAsCreated(Guid? userId)
    {
        var now = DateTime.UtcNow;
        CreatedBy = userId;
        CreatedAt = now;
        UpdatedAt = now;
        UpdatedBy = userId;
    }

    public void MarkAsUpdated(Guid? userId)
    {
        UpdatedBy = userId;
        UpdatedAt = DateTime.UtcNow;
    }
}
