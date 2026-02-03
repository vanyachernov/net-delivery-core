using MediatR;
using Workers.Application.Users.DTOs;

namespace Workers.Application.Users.Queries.GetUsersList;

public record GetUsersListQuery(int Page = 1, int PageSize = 20) : IRequest<List<UserDto>>;