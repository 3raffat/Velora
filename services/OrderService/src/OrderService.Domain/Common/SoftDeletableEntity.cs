namespace OrderService.Domain.Common;

public abstract class SoftDeletableEntity : AuditableEntity
{
    protected SoftDeletableEntity() { }

    protected SoftDeletableEntity(Guid id)
        : base(id) { }

    public bool IsDeleted { get; protected set; }
    public Guid? DeletedBy { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }

    public void MarkAsDeleted(Guid? id)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = id;
    }
}
