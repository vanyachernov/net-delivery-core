using MediatR;
using Microsoft.AspNetCore.Mvc;
using Workers.Application.Users.Commands.CreateUser;
using Workers.Application.Users.Queries.GetUserById;
using Workers.Application.Users.Queries.GetUsersList;

namespace Workers.Api.Controllers;

public class UsersController(IMediator mediator) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserCommand command, 
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(command, cancellationToken);
        return OkResult(result);
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetById(
        Guid id, 
        CancellationToken cancellationToken = default)
    {
        var userDataDto = await mediator.Send(
            new GetUserByIdQuery(id), 
            cancellationToken );
        
        return userDataDto is null 
            ? NotFound() 
            : OkResult(userDataDto);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetUsersListQuery(page, pageSize), cancellationToken);
        return OkResult(result);
    }
}