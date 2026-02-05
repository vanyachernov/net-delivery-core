using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Workers.Application.Users.Commands.CreateUser;
using Workers.Application.Users.Queries.GetUserById;
using Workers.Application.Users.Queries.GetUsersList;

namespace Workers.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/users")]
public class UsersController(IMediator mediator) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserCommand command, 
        CancellationToken ct)
    {
        var data = await mediator.Send(command, ct);
        
        return OkResult(data);
    }
        

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id, 
        CancellationToken ct)
    {
        var user = await mediator.Send(new GetUserByIdQuery(id), ct);

        if (user is null)
        {
            return NotFoundResult();
        }
        
        return OkResult(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        return OkResult(await mediator.Send(new GetUsersListQuery(page, pageSize), ct));
    }
}
    