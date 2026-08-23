using DeliveryService.Application.Common.Exceptions;
using FluentValidation;
using MediatR;

namespace DeliveryService.Application.Common.Behaviors;

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

        throw new DeliveryService.Application.Common.Exceptions.ValidationException(
            validationResult.Errors
        );
    }
}
