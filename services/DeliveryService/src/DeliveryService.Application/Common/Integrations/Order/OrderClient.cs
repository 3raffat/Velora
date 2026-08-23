using DeliveryService.Application.Common.Interfaces;

namespace DeliveryService.Application.Common.Integrations.Order;

public sealed class OrderClient(HttpClient httpClient) : IOrderClient
{
    public async Task DeliverOrderAsync(Guid orderId, CancellationToken ct = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Put,
            $"api/v1/orders/{orderId}/deliver"
        );
        using var response = await httpClient.SendAsync(request, ct);

        response.EnsureSuccessStatusCode();
    }
}
