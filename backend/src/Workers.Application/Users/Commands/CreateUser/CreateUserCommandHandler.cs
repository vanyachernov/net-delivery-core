using Workers.Application.Common.Models;

namespace Workers.Application.Users.Commands.CreateUser;

using MediatR;
using Common.Interfaces;
using DTOs;
using Workers.Domain.Entities.Users;
using Microsoft.Extensions.Logging;

public class CreateUserCommandHandler(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork,
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
        var phone = request.PhoneNumber.Trim();
        var role = request.Role.ToString();

        // 1. Create Domain User instance to generate ID
        var newUser = new User
        {
            FirstName = request.FirstName?.Trim(),
            LastName = request.LastName?.Trim(),
            AvatarUrl = null
        };
        
        // 2. Create Identity User with same ID
        var identityDto = new CreateIdentityUserDto(newUser.Id, email, request.Password, phone, role);
        var (identityId, error) = await identityService.CreateUserAsync(identityDto);

        if (!string.IsNullOrEmpty(error))
        {
            logger.LogError("Failed to create identity user: {Error}", error);
            throw new InvalidOperationException($"Failed to create user: {error}");
        }

        // 3. Save Domain User
        await userRepository.AddAsync(newUser, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Map DTO manually or via mapper. 
        return new UserDto(
            newUser.Id,
            email,
            phone,
            newUser.FirstName,
            newUser.LastName,
            request.Role,
            false,
            false,
            newUser.AvatarUrl
        );
    }
}
