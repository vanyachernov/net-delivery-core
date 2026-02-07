using MediatR;
using Microsoft.Extensions.Logging;
using Workers.Application.Users.DTOs;

namespace Workers.Application.Users.Queries.GetUsersList;

public class GetUsersListQueryHandler(
    IUserRepository users,
    ILogger<GetUsersListQueryHandler> logger) 
    : IRequestHandler<GetUsersListQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetUsersListQuery request, CancellationToken ct)
    {
        logger.LogInformation("Retrieving paged users list. Page: {Page}, PageSize: {PageSize}", request.Page, request.PageSize);

        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 20 : request.PageSize;
        
        if (pageSize > 200)
        {
            logger.LogInformation("Requested PageSize {RequestedSize} exceeded limit, capping at 200", pageSize);
            pageSize = 200;
        }

        var userEntities = await users.GetPagedAsync(page, pageSize, ct);

        logger.LogInformation("Successfully retrieved {Count} users for page {Page}", userEntities.Count, page);

        return userEntities.Select(u => new UserDto(
            u.Id,
            u.Email ?? string.Empty,
            u.PhoneNumber ?? string.Empty,
            u.FirstName,
            u.LastName,
            u.Role,
            u.EmailConfirmed,
            u.PhoneNumberConfirmed,
            u.AvatarUrl
        )).ToList();
    }
}