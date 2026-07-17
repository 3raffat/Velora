namespace Velora.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; }

    protected BaseEntity() { }

    public BaseEntity(Guid id)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
    }
}
