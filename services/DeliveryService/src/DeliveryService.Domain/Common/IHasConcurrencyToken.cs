namespace DeliveryService.Domain.Common;

public interface IHasConcurrencyToken
{
    byte[] Version { get; }
}
