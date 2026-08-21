using DeliveryService.Application.Common.Exceptions;
using DeliveryService.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryService.Api.Middleware;

public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger,
    IHostEnvironment environment
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken ct
    )
    {
        logger.LogError(
            exception,
            "Unhandled exception occurred: {Message}. TraceId: {TraceId}",
            exception.Message,
            httpContext.TraceIdentifier
        );

        var (statusCode, title) = MapException(exception);
        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{statusCode}",
            Title = title,
            Status = statusCode,
            Detail = environment.IsDevelopment() || statusCode < 500 ? exception.Message : null,
        };

        if (exception is ValidationException validation)
            problemDetails.Extensions["errors"] = validation.Errors;

        return await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = problemDetails,
            }
        );
    }

    private static (int StatusCode, string Title) MapException(Exception exception) =>
        exception switch
        {
            ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
            NotFoundException => (StatusCodes.Status404NotFound, "Resource Not Found"),
            ValidationException => (StatusCodes.Status400BadRequest, "Validation Error"),
            InvalidRequestException => (StatusCodes.Status400BadRequest, "Invalid Request"),
            UnauthorizedException => (StatusCodes.Status401Unauthorized, "Unauthorized Access"),
            OperationException => (StatusCodes.Status500InternalServerError, "Operation Failed"),
            DomainException => (StatusCodes.Status400BadRequest, "Business Rule Violation"),
            ArgumentException => (StatusCodes.Status400BadRequest, "Invalid Request"),
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized Access"
            ),
            _ => (StatusCodes.Status500InternalServerError, "An Unexpected Error Occurred"),
        };
}
