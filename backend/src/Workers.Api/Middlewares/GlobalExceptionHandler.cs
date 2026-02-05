using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Workers.Api.Models;

namespace Workers.Api.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var (statusCode, response) = exception switch
        {
            ValidationException validationException => HandleValidationException(validationException),
            _ => HandleDefaultException(exception)
        };

        httpContext.Response.StatusCode = statusCode;
        
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }

    private static (int StatusCode, object Response) HandleValidationException(ValidationException exception)
    {
        var errors = exception.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );

        return ((int)HttpStatusCode.BadRequest, ApiResult.Failure(new
        {
            message = "Validation failed",
            errors
        }));
    }

    private static (int StatusCode, object Response) HandleDefaultException(Exception exception)
    {
        return ((int)HttpStatusCode.InternalServerError, ApiResult.Failure(new
        {
            message = "An internal server error occurred."
        }));
    }
}
