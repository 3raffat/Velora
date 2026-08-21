namespace DeliveryService.Application.Common.Exceptions;

public sealed class InvalidRequestException(string message) : AppException(message);
