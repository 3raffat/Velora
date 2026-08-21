namespace DeliveryService.Domain.Common;

public abstract class SoftDeletableEntity : AuditableEntity
{
    protected SoftDeletableEntity() { }

    protected SoftDeletableEntity(Guid id)
        : base(id) { }

    public bool IsDeleted { get; private set; }
    public Guid? DeletedBy { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public void MarkAsDeleted(Guid? userId)
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
        DeletedBy = userId;
    }
}
