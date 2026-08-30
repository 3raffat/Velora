namespace OrderService.Application.Common.Interfaces;

public interface IPayPalClient
{
    Task<string> CaptureOrderAsync(string paypalOrderId, CancellationToken ct = default);
    Task<string> RefundCaptureAsync(
        string captureId,
        decimal amount,
        string currencyCode = "USD",
        CancellationToken ct = default
    );
}
