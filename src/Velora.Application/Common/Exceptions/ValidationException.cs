using Velora.Application.Common.Models;

namespace Velora.Application.Common.Exceptions;

public sealed class ValidationException : Exception
{
    public IReadOnlyCollection<ValidationError> Errors { get; }

    public ValidationException(IEnumerable<ValidationError> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors.ToList().AsReadOnly();
    }
}
