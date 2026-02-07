namespace Workers.Application.Users.Commands.CreateUser;

using MediatR;
using Workers.Application.Common.Interfaces;
using Workers.Application.Identity;
using Workers.Application.Identity.DTOs;
using Workers.Application.Users;
using DTOs;
using Microsoft.Extensions.Logging;

public class CreateUserCommandHandler(
    IIdentityService identityService,
    ILogger<CreateUserCommandHandler> logger)
    : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(
        CreateUserCommand request, 
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting the process of creating a new user with email: {Email}", request.Email);
        
        var email = request.Email.Trim().ToLowerInvariant();
        var phone = request.PhoneNumber?.Trim() ?? string.Empty;
        var role = request.Role.ToString();

        var createUserDto = new CreateUserDto(
            email,
            request.Password,
            role,
            request.FirstName?.Trim(),
            request.LastName?.Trim(),
            phone,
            null
        );

        var result = await identityService.CreateUserAsync(createUserDto);

        if (!result.Succeeded)
        {
            logger.LogError("Failed to create user: {Error}", result.Error);
            throw new InvalidOperationException($"Failed to create user: {result.Error}");
        }

        return new UserDto(
            Guid.Parse(result.UserId!),
            email,
            phone,
            createUserDto.FirstName,
            createUserDto.LastName,
            request.Role,
            false,
            false,
            createUserDto.AvatarUrl
        );
    }
}
