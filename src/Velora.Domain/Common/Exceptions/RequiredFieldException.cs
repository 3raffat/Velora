namespace Velora.Domain.Common.Exceptions;

public sealed class RequiredFieldException(string fieldName)
    : DomainException($"{fieldName} is required") { }
