using MediatR;
using Microsoft.AspNetCore.Mvc;
using Workers.Application.Users.Commands.CreateUser;
using Workers.Application.Users.Queries.GetUserById;
using Workers.Application.Users.Queries.GetUsersList;

namespace Workers.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IMediator mediator) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserCommand userCommand, 
        CancellationToken cancellationToken = default)
    {
        var userDataDto = await mediator.Send(
            userCommand, 
            cancellationToken);
        
        return OkResult(userDataDto);
    }
        

    [HttpGet("{id:guid}")]
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

    [HttpGet]
    public async Task<IActionResult> GetList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        var usersDataDto = await mediator.Send(
            new GetUsersListQuery(page, pageSize), 
            cancellationToken);
        
        return OkResult(usersDataDto);
    }
}
    