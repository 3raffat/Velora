using System.Net.Http.Json;
using Velora.Application.Common.Interfaces;
using Velora.Application.Common.Models;

namespace Velora.Application.Common.Integrations.Delivery;

public sealed class DeliveryClient(HttpClient _httpClient) : IDeliveryClient
{
    public async Task<CreateShipmentResponse> CreateShipmentAsync(
        CreateShipmentRequest request,
        CancellationToken ct = default
    )
    {
        var response = await _httpClient.PostAsJsonAsync("api/v1/shipments", request, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<CreateShipmentResponse>(ct);

        return result
            ?? throw new InvalidOperationException("Delivery service returned an empty response.");
    }
}
