namespace DeliveryService.Domain.Common.Exceptions;

public sealed class RequiredFieldException(string fieldName)
    : DomainException($"The field '{fieldName}' is required.");
