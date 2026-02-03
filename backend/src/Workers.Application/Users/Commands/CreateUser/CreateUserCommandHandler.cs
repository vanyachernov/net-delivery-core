namespace Workers.Application.Users.Commands.CreateUser;

using MediatR;
using Workers.Application.Common.Interfaces;
using Workers.Application.Users.DTOs;
using Workers.Domain.Entities.Users;


public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _uow;

    public CreateUserCommandHandler(IUserRepository users, IUnitOfWork uow)
    {
        _users = users;
        _uow = uow;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var phone = request.PhoneNumber.Trim();

        if (await _users.EmailExistsAsync(email, ct))
            throw new InvalidOperationException("User with this email already exists.");

        if (await _users.PhoneExistsAsync(phone, ct))
            throw new InvalidOperationException("User with this phone already exists.");

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

        await _users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

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
