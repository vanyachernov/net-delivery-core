using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Workers.Api.Models;
using Workers.Domain.Constants;
using Workers.Domain.Exceptions;

namespace Workers.Api.Middlewares;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger, 
    IHostEnvironment environment) : IExceptionHandler
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
            NotFoundException notFoundException => HandleNotFoundException(notFoundException),
            BadRequestException badRequestException => HandleBadRequestException(badRequestException),
            ConflictException conflictException => HandleConflictException(conflictException),
            UnauthorizedException unauthorizedException => HandleUnauthorizedException(unauthorizedException),
            ForbiddenException forbiddenException => HandleForbiddenException(forbiddenException),
            _ => HandleDefaultException(exception, environment.IsDevelopment())
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

        var apiError = ApiError.Validation("Validation failed", errors);

        return ((int)HttpStatusCode.BadRequest, ApiResult.Failure(apiError));
    }

    private static (int StatusCode, object Response) HandleNotFoundException(NotFoundException exception)
    {
        var apiError = ApiError.Simple(exception.Message, exception.Code);
        return ((int)HttpStatusCode.NotFound, ApiResult.Failure(apiError));
    }

    private static (int StatusCode, object Response) HandleBadRequestException(BadRequestException exception)
    {
        var apiError = ApiError.Simple(exception.Message, exception.Code);
        return ((int)HttpStatusCode.BadRequest, ApiResult.Failure(apiError));
    }

    private static (int StatusCode, object Response) HandleConflictException(ConflictException exception)
    {
        var apiError = ApiError.Simple(exception.Message, exception.Code);
        return ((int)HttpStatusCode.Conflict, ApiResult.Failure(apiError));
    }

    private static (int StatusCode, object Response) HandleUnauthorizedException(UnauthorizedException exception)
    {
        var apiError = ApiError.Simple(exception.Message, exception.Code);
        return ((int)HttpStatusCode.Unauthorized, ApiResult.Failure(apiError));
    }

    private static (int StatusCode, object Response) HandleForbiddenException(ForbiddenException exception)
    {
        var apiError = ApiError.Simple(exception.Message, exception.Code);
        return ((int)HttpStatusCode.Forbidden, ApiResult.Failure(apiError));
    }

    private static (int StatusCode, object Response) HandleDefaultException(Exception exception, bool isDevelopment)
    {
        if (isDevelopment)
        {
            var apiError = new ApiError(
                message: exception.Message,
                code: "INTERNAL_ERROR",
                validationErrors: null,
                details: new
                {
                    type = exception.GetType().Name,
                    stackTrace = exception.StackTrace,
                    innerException = exception.InnerException?.Message
                });

            return ((int)HttpStatusCode.InternalServerError, ApiResult.Failure(apiError));
        }

        var genericError = ApiError.Simple("An internal server error occurred.", "INTERNAL_ERROR");
        return ((int)HttpStatusCode.InternalServerError, ApiResult.Failure(genericError));
    }
}
