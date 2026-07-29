using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.Common.Exceptions;
using Velora.Domain.Common.Exceptions;

namespace Velora.Api.Middleware;

public sealed class GlobalExceptionHandler(
    IProblemDetailsService _service,
    ILogger<GlobalExceptionHandler> _logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken ct
    )
    {
        _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        var (statusCode, title) = exception switch
        {
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

        var problem = new ProblemDetailsContext()
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails()
            {
                Title = title,
                Status = statusCode,
                Detail = exception.Message,
            },
        };

        httpContext.Response.StatusCode = statusCode;

        await _service.TryWriteAsync(problem);
        return true;
    }
}
