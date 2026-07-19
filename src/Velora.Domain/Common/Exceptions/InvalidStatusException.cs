namespace Velora.Domain.Common.Exceptions;

public sealed class InvalidStatusException : DomainException
{
    public InvalidStatusException(string message)
        : base(message) { }

    public InvalidStatusException(
        string entity,
        string operation,
        Enum currentStatus,
        Enum expectedStatus
    )
        : base(
            $"Cannot {operation} {entity.ToLower()} because its status is '{currentStatus}'. Expected status: '{expectedStatus}'."
        ) { }
}
