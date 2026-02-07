using MediatR;
using Microsoft.AspNetCore.Mvc;
using Workers.Application.Identity.Commands.Login;
using Workers.Application.Identity.Commands.Register;

namespace Workers.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ApiControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await mediator.Send(command);
        
        if (!result.Succeeded)
        {
            return UnauthorizedResult(new { error = result.Error });
        }

        return OkResult(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await mediator.Send(command);

        if (!result.Succeeded)
        {
            return BadRequestResult(new { error = result.Error });
        }
        
        return OkResult(result);
    }
}
