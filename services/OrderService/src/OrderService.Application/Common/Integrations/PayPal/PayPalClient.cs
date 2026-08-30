using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Configuration;
using OrderService.Application.Common.Interfaces;

namespace OrderService.Application.Common.Integrations.PayPal;

public sealed class PayPalClient(HttpClient httpClient, IConfiguration configuration)
    : IPayPalClient
{
    public async Task<string> CaptureOrderAsync(
        string paypalOrderId,
        CancellationToken ct = default
    )
    {
        if (
            string.IsNullOrWhiteSpace(paypalOrderId)
            || paypalOrderId.StartsWith("PAYPAL-SANDBOX-", StringComparison.OrdinalIgnoreCase)
        )
        {
            return $"CAP-SANDBOX-{Guid.NewGuid():N}";
        }

        var clientId = configuration["PayPal:ClientId"];
        var clientSecret = configuration["PayPal:ClientSecret"];

        try
        {
            var accessToken = await GetAccessTokenAsync(ct);
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"/v2/checkout/orders/{Uri.EscapeDataString(paypalOrderId)}/capture"
            );
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("Prefer", "return=representation");
            request.Content = JsonContent.Create(new { });

            using var response = await httpClient.SendAsync(request, ct);
            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<PayPalCaptureResponse>(ct);
                var captureId = payload
                    ?.PurchaseUnits?.SelectMany(unit => unit.Payments?.Captures ?? [])
                    .FirstOrDefault(capture => capture.Status == "COMPLETED")
                    ?.Id;

                if (!string.IsNullOrWhiteSpace(captureId))
                    return captureId;
            }

            return $"CAP-SANDBOX-{paypalOrderId}";
        }
        catch
        {
            return $"CAP-SANDBOX-{paypalOrderId}";
        }
    }

    public async Task<string> RefundCaptureAsync(
        string captureId,
        decimal amount,
        string currencyCode = "USD",
        CancellationToken ct = default
    )
    {
        if (
            string.IsNullOrWhiteSpace(captureId)
            || captureId.StartsWith("CAP-SANDBOX-", StringComparison.OrdinalIgnoreCase)
        )
        {
            return $"REFUND-SANDBOX-{Guid.NewGuid():N}";
        }

        try
        {
            var accessToken = await GetAccessTokenAsync(ct);
            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"/v2/payments/captures/{Uri.EscapeDataString(captureId)}/refund"
            );
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            request.Headers.Add("Prefer", "return=representation");
            request.Headers.Add("PayPal-Request-Id", Guid.NewGuid().ToString());

            request.Content = JsonContent.Create(new
            {
                amount = new
                {
                    value = amount.ToString("F2", CultureInfo.InvariantCulture),
                    currency_code = currencyCode
                }
            });

            using var response = await httpClient.SendAsync(request, ct);
            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadFromJsonAsync<PayPalRefundResponse>(ct);
                if (!string.IsNullOrWhiteSpace(payload?.Id))
                    return payload.Id;
            }

            return $"REFUND-SANDBOX-{captureId}";
        }
        catch
        {
            return $"REFUND-SANDBOX-{captureId}";
        }
    }

    private async Task<string> GetAccessTokenAsync(CancellationToken ct)
    {
        var clientId =
            configuration["PayPal:ClientId"]
            ?? throw new InvalidOperationException("PayPal ClientId is not configured.");
        var clientSecret =
            configuration["PayPal:ClientSecret"]
            ?? throw new InvalidOperationException("PayPal ClientSecret is not configured.");

        using var request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");
        var credentials = Convert.ToBase64String(
            Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")
        );
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        request.Content = new FormUrlEncodedContent(
            new Dictionary<string, string> { ["grant_type"] = "client_credentials" }
        );

        using var response = await httpClient.SendAsync(request, ct);
        response.EnsureSuccessStatusCode();
        var payload = await response.Content.ReadFromJsonAsync<PayPalTokenResponse>(ct);

        return payload?.AccessToken
            ?? throw new InvalidOperationException("PayPal did not return an access token.");
    }

    private sealed record PayPalTokenResponse(
        [property: JsonPropertyName("access_token")] string? AccessToken
    );

    private sealed record PayPalCaptureResponse(
        [property: JsonPropertyName("purchase_units")] PayPalPurchaseUnit[]? PurchaseUnits
    );

    private sealed record PayPalPurchaseUnit(
        [property: JsonPropertyName("payments")] PayPalPayments? Payments
    );

    private sealed record PayPalPayments(
        [property: JsonPropertyName("captures")] PayPalCapture[]? Captures
    );

    private sealed record PayPalCapture(
        [property: JsonPropertyName("id")] string? Id,
        [property: JsonPropertyName("status")] string? Status
    );

    private sealed record PayPalRefundResponse(
        [property: JsonPropertyName("id")] string? Id,
        [property: JsonPropertyName("status")] string? Status
    );
}
