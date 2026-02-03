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

    protected IActionResult BadRequestResult(object? errors)
    {
        return BadRequest(ApiResult.Failure(errors));
    }

    protected IActionResult NotFoundResult(object? errors = null)
    {
        return NotFound(ApiResult.Failure(errors ?? new { message = "Resource not found" }));
    }
}
