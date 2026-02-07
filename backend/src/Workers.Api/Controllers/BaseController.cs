using Microsoft.AspNetCore.Mvc;
using Workers.Api.Models;

namespace Workers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected IActionResult OkResult<T>(T data)
    {
        return Ok(ApiResult.Success(data));
    }

    protected IActionResult CreatedResult<T>(string actionName, object routeValues, T data)
    {
        return CreatedAtAction(actionName, routeValues, ApiResult.Success(data));
    }

    protected IActionResult BadRequestResult(string message, string? code = null)
    {
        return BadRequest(ApiResult.Failure(message, code));
    }

    protected IActionResult BadRequestResult(ApiError error)
    {
        return BadRequest(ApiResult.Failure(error));
    }

    protected IActionResult NotFoundResult(string message = "Resource not found")
    {
        return NotFound(ApiResult.Failure(message, "NOT_FOUND"));
    }

    protected IActionResult UnauthorizedResult(string message)
    {
        return NotFound(ApiResult.Failure(error));
    }

    protected IActionResult UnauthorizedResult(string message)
    {
        return Unauthorized(ApiResult.Failure(message, "UNAUTHORIZED"));
    }
}
