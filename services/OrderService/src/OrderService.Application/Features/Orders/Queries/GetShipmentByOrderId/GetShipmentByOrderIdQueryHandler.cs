using MediatR;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Models;

namespace OrderService.Application.Features.Orders.Queries.GetShipmentByOrderId;

public sealed class GetShipmentByOrderIdQueryHandler(IDeliveryClient _deliveryClient)
    : IRequestHandler<GetShipmentByOrderIdQuery, ShipmentTrackingResponse>
{
    public Task<ShipmentTrackingResponse> Handle(
        GetShipmentByOrderIdQuery request,
        CancellationToken ct
    )
    {
        return _deliveryClient.GetShipmentByOrderIdAsync(request.OrderId, ct);
    }
}
