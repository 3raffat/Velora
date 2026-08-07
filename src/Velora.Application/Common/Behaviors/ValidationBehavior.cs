using FluentValidation;
using MediatR;
using Velora.Application.Common.Models;
using AppValidationException = Velora.Application.Common.Exceptions.ValidationException;

namespace Velora.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IValidator<TRequest>? _validator = null)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct
    )
    {
        if (_validator is null)
            return await next(ct);

        var validationResult = await _validator.ValidateAsync(request, ct);

        if (validationResult.IsValid)
            return await next(ct);

        var errors = validationResult.Errors.ConvertAll(e => new ValidationError(
            e.ErrorMessage,
            e.PropertyName
        ));

        throw new AppValidationException(errors);
    }
}
