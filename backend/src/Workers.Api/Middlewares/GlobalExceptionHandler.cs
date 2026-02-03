using System.Net;
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
        logger.LogError(exception, 
            "An unhandled exception occurred: {Message}", exception.Message);

        var response = ApiResult.Failure(new
        {
            message = "An internal server error occurred."
        });
        
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        
        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

        return true;
    }
}
