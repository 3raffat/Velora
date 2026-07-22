namespace Velora.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; protected set; }

    public Guid? CreatedBy { get; protected set; }

    public DateTime? UpdatedAt { get; protected set; }

    public Guid? UpdatedBy { get; protected set; }

    protected AuditableEntity() { }

    protected AuditableEntity(Guid id)
        : base(id) { }

    public void MarkAsCreated(Guid? id)
    {
        var now = DateTime.UtcNow;
        CreatedBy = id;
        CreatedAt = now;
        UpdatedAt = now;
        UpdatedBy = id;
    }

    public void MarkAsUpdated(Guid? id)
    {
        UpdatedBy = id;
        UpdatedAt = DateTime.UtcNow;
    }
}
