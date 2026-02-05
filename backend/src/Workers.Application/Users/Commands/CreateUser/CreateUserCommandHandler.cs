namespace Workers.Application.Users.Commands.CreateUser;

using MediatR;
using Common.Interfaces;
using DTOs;
using Workers.Domain.Entities.Users;

public class CreateUserCommandHandler(
    IUserRepository userRepository, 
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(
        CreateUserCommand request, 
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting the process of creating a new user with email: {Email}", request.Email);
        
        var email = request.Email.Trim().ToLowerInvariant();
        var phone = request.PhoneNumber.Trim();

        if (await userRepository.EmailExistsAsync(email, cancellationToken))
        {
            throw new InvalidOperationException("User with this email already exists.");
        }

        if (await userRepository.PhoneExistsAsync(phone, cancellationToken))
        {
            throw new InvalidOperationException("User with this phone already exists.");
        }

        var newUser = new User
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

        await userRepository.AddAsync(newUser, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserDto(
            newUser.Id,
            newUser.Email,
            newUser.PhoneNumber,
            newUser.FirstName,
            newUser.LastName,
            newUser.Role,
            newUser.IsEmailVerified,
            newUser.IsPhoneVerified,
            newUser.AvatarUrl
        );
    }
}
