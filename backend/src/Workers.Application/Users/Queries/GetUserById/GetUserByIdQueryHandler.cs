using MediatR;
using Workers.Application.Common.Interfaces;
using Workers.Application.Users.DTOs;

namespace Workers.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _users;

    public GetUserByIdQueryHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken ct)
    {
        var user = await _users.GetByIdAsync(request.Id, ct);
        if (user is null) return null;

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