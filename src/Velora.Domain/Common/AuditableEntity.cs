namespace Velora.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    protected AuditableEntity() { }

    protected AuditableEntity(Guid id)
        : base(id) { }

    protected void MarkAsCreated()
    {
        var now = DateTime.UtcNow;

        CreatedAt = now;
        UpdatedAt = now;
    }

    protected void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
