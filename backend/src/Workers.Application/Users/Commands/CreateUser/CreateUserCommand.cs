namespace Workers.Application.Users.Commands.CreateUser;

using MediatR;
using DTOs;
using Domain.Enums;

public record CreateUserCommand(
    string Email,
    string Password,
    string PhoneNumber,
    string? FirstName,
    string? LastName,
    UserRole Role
) : IRequest<UserDto>;
