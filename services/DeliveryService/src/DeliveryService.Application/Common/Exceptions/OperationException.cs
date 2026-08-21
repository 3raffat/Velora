namespace DeliveryService.Application.Common.Exceptions;

public sealed class OperationException(string message) : AppException(message);
