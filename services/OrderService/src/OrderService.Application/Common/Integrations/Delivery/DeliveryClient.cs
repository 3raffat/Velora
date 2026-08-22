using System.Net.Http.Json;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Models;
using OrderService.Application.Common.Response;

namespace OrderService.Application.Common.Integrations.Delivery;

public sealed class DeliveryClient(HttpClient _httpClient) : IDeliveryClient
{
    public async Task<CreateShipmentResponse> CreateShipmentAsync(
        CreateShipmentRequest request,
        CancellationToken ct = default
    )
    {
        var deliveryRequest = new
        {
            request.OrderId,
            RecipientName = request.CustomerName,
            RecipientPhone = request.CustomerPhone,
            AddressLine1 = request.ShippingAddress.AddressLine1,
            AddressLine2 = request.ShippingAddress.AddressLine2,
            City = request.ShippingAddress.City,
            State = request.ShippingAddress.State,
            Country = request.ShippingAddress.Country,
            request.TotalAmount,
        };

        var response = await _httpClient.PostAsJsonAsync("api/v1/shipments", deliveryRequest, ct);
        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<
            StandardSuccessResponse<CreateShipmentResponse>
        >(ct);

        return payload?.Data
            ?? throw new InvalidOperationException(
                "Delivery service returned an empty response payload."
            );
    }
}
