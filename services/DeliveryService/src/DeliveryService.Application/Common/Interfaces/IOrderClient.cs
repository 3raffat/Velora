namespace DeliveryService.Application.Common.Interfaces;

public interface IOrderClient
{
    Task DeliverOrderAsync(Guid orderId, CancellationToken ct = default);
}
