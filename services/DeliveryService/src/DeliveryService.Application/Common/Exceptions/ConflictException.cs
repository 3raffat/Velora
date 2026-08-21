namespace DeliveryService.Application.Common.Exceptions;

public sealed class ConflictException(string message) : AppException(message);
