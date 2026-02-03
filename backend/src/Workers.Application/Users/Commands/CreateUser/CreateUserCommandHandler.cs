namespace Workers.Application.Users.Commands.CreateUser;

using MediatR;
using Microsoft.Extensions.Logging;
using Common.Interfaces;
using DTOs;
using Workers.Domain.Entities.Users;


public class CreateUserCommandHandler(
    IUserRepository users, 
    IUnitOfWork uow,
    ILogger<CreateUserCommandHandler> logger)
    : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(
        CreateUserCommand request, 
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting the process of creating a new user with email: {Email}", request.Email);
        
        var email = request.Email.Trim().ToLowerInvariant();
        var phone = request.PhoneNumber.Trim();

        logger.LogInformation("Checking if email or phone already exists...");

        if (await users.EmailExistsAsync(email, cancellationToken))
        {
            logger.LogWarning("Failed to create user: Email {Email} already exists", email);
            throw new InvalidOperationException("User with this email already exists.");
        }

        if (await users.PhoneExistsAsync(phone, cancellationToken))
        {
            logger.LogWarning("Failed to create user: Phone {Phone} already exists", phone);
            throw new InvalidOperationException("User with this phone already exists.");
        }

        var user = new User
        {
            Email = email,
            PhoneNumber = phone,
            FirstName = request.FirstName?.Trim(),
            LastName = request.LastName?.Trim(),
            Role = request.Role,
            IsEmailVerified = false,
            IsPhoneVerified = false,
            AvatarUrl = null
        };

        logger.LogInformation("Creating user entity and adding to repository...");

        await users.AddAsync(user, cancellationToken);
        
        logger.LogInformation("Saving changes to the database...");
        await uow.SaveChangesAsync(cancellationToken);

        logger.LogInformation("User created successfully with ID: {UserId}", user.Id);

        return new UserDto(
            user.Id,
            user.Email,
            user.PhoneNumber,
            user.FirstName,
            user.LastName,
            user.Role,
            user.IsEmailVerified,
            user.IsPhoneVerified,
            user.AvatarUrl
        );
    }
}
