using MediatR;
using Workers.Application.Common.Interfaces;
using Workers.Application.Users.DTOs;

namespace Workers.Application.Users.Queries.GetUsersList;

public class GetUsersListQueryHandler : IRequestHandler<GetUsersListQuery, List<UserDto>>
{
    private readonly IUserRepository _users;

    public GetUsersListQueryHandler(IUserRepository users)
    {
        _users = users;
    }

    public async Task<List<UserDto>> Handle(GetUsersListQuery request, CancellationToken ct)
    {
        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;
        if (pageSize > 200) pageSize = 200;

        var users = await _users.GetPagedAsync(page, pageSize, ct);

        return users.Select(u => new UserDto(
            u.Id,
            u.Email,
            u.PhoneNumber,
            u.FirstName,
            u.LastName,
            u.Role,
            u.IsEmailVerified,
            u.IsPhoneVerified,
            u.AvatarUrl
        )).ToList();
    }
}