using Velora.Application.Common.Models;

namespace Velora.Application.Common.Interfaces;

public interface IDeliveryClient
{
    Task<CreateShipmentResponse> CreateShipmentAsync(
        CreateShipmentRequest request,
        CancellationToken ct = default
    );
}
