namespace DeliveryService.Clients;

public sealed class VeloraClient(HttpClient httpClient)
{
    public async Task NotifyDeliveredAsync(
        Guid orderId,
        Guid shipmentId,
        string trackingNumber,
        CancellationToken ct
    )
    {
        var request = new
        {
            OrderId = orderId,
            ShipmentId = shipmentId,
            TrackingNumber = trackingNumber,
        };

        var response = await httpClient.PostAsJsonAsync(
            $"api/v1/orders/{orderId}/deliver",
            request,
            ct
        );

        response.EnsureSuccessStatusCode();
    }
}
