using DeliveryService.Application.Common.Interfaces;
using DeliveryService.Domain.Entities.Shipments.Events;
using MediatR;

namespace DeliveryService.Application.Features.Shipments.Events;

public sealed class ShipmentDeliveredEventHandler(IOrderClient orderClient)
    : INotificationHandler<ShipmentDeliveredEvent>
{
    public Task Handle(ShipmentDeliveredEvent notification, CancellationToken ct)
    {
        return orderClient.DeliverOrderAsync(notification.OrderId, ct);
    }
}
