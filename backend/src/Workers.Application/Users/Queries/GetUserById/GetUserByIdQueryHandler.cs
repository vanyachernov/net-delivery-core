using MediatR;
using Microsoft.Extensions.Logging;
using Workers.Application.Common.Interfaces;
using Workers.Application.Users;
using Workers.Application.Users.DTOs;

namespace Workers.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler(
    IUserRepository users,
    ILogger<GetUserByIdQueryHandler> logger) 
    : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        logger.LogInformation("Retrieving user by ID: {UserId}", request.Id);
        
        var user = await users.GetByIdAsync(request.Id, ct);
        
        if (user is null)
        {
            logger.LogWarning("User with ID {UserId} was not found", request.Id);
            return null;
        }

        logger.LogInformation("User with ID {UserId} successfully retrieved", request.Id);

        return new UserDto(
            user.Id,
            user.Email ?? string.Empty,
            user.PhoneNumber ?? string.Empty,
            user.FirstName,
            user.LastName,
            user.Role,
            user.EmailConfirmed,
            user.PhoneNumberConfirmed,
            user.AvatarUrl
        );
    }
}