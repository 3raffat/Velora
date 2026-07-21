namespace Velora.Domain.Common;

public interface IHasConcurrencyToken
{
    byte[] Version { get; }
}
