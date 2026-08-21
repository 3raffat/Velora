namespace DeliveryService.Application.Common.Exceptions;

public sealed class UnauthorizedException(string message) : AppException(message);
