using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Common.Exceptions;
using Velora.Domain.Common.Exceptions;

namespace Velora.Api.Middleware;

public sealed class GlobalExceptionHandler(
    IProblemDetailsService _service,
    ILogger<GlobalExceptionHandler> _logger,
    IHostEnvironment _env
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken ct
    )
    {
        _logger.LogError(
            exception,
            "Unhandled exception occurred: {Message} traceId: {TraceId}",
            exception.Message,
            httpContext.TraceIdentifier
        );

        var (statusCode, title) = MapException(exception);

        var problem = new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Type = $"https://httpstatuses.com/{statusCode}",
                Title = title,
                Status = statusCode,
                Detail = GetSafeErrorMessage(exception, statusCode),
            },
        };

        httpContext.Response.StatusCode = statusCode;

        return await _service.TryWriteAsync(problem);
    }

    private static (int statusCode, string title) MapException(Exception exception) =>
        exception switch
        {
            AuthException => (StatusCodes.Status400BadRequest, "Authentication Error"),

            DomainException => (StatusCodes.Status400BadRequest, "Business Rule Violation"),

            ValidationException => (StatusCodes.Status400BadRequest, "Validation Error"),

            NotFoundException => (StatusCodes.Status404NotFound, "Resource Not Found"),

            ConflictException => (StatusCodes.Status409Conflict, "Resource Conflict"),

            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized Access"
            ),
            _ => (StatusCodes.Status500InternalServerError, "An Unexpected Error Occurred"),
        };

    private string? GetSafeErrorMessage(Exception exception, int statusCode)
    {
        if (_env.IsDevelopment())
            return exception.Message;

        return statusCode >= StatusCodes.Status500InternalServerError ? null : exception.Message;
    }
}
