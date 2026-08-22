using OrderService.Application.Common.Models;

namespace OrderService.Application.Common.Interfaces;

public interface IDeliveryClient
{
    Task<CreateShipmentResponse> CreateShipmentAsync(
        CreateShipmentRequest request,
        CancellationToken ct = default
    );

    Task<ShipmentTrackingResponse> GetShipmentByOrderIdAsync(
        Guid orderId,
        CancellationToken ct = default
    );
}
