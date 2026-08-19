namespace DeliveryService.Responces;

public sealed record StandardSuccessResponse<T>(T? Data, int Status, string Message);
