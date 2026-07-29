namespace Velora.Application.Common.Response;

public sealed record StandardSuccessResponse<T>(T? Data, int Status, string Message);
