using MediatR;
using Workers.Application.Users.DTOs;

namespace Workers.Application.Users.Queries.GetUserById;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;