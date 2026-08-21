namespace DeliveryService.Domain.Common.Exceptions;

public sealed class InvalidValueException(string message) : DomainException(message);
