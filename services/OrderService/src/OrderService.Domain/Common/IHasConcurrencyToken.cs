namespace OrderService.Domain.Common;

public interface IHasConcurrencyToken
{
    byte[] Version { get; }
}
