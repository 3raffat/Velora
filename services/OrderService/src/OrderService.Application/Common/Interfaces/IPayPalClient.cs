namespace OrderService.Application.Common.Interfaces;

public interface IPayPalClient
{
    Task<string> CaptureOrderAsync(string paypalOrderId, CancellationToken ct = default);
}
