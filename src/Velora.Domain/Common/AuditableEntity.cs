namespace Velora.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; protected set; }

    public Guid? CreatedBy { get; protected set; }

    public DateTime? UpdatedAt { get; protected set; }

    public Guid? UpdatedBy { get; protected set; }

    public bool IsDeleted { get; protected set; }

    public DateTime? DeletedAt { get; protected set; }

    public Guid? DeletedBy { get; protected set; }

    protected AuditableEntity() { }

    protected AuditableEntity(Guid id)
        : base(id) { }

    public void MarkAsCreated(Guid? Id)
    {
        var now = DateTime.UtcNow;
        CreatedBy = Id;
        CreatedAt = now;
        UpdatedAt = now;
        UpdatedBy = Id;
    }

    public void MarkAsUpdated(Guid? Id)
    {
        UpdatedBy = Id;
        UpdatedAt = DateTime.UtcNow;
    }
}
