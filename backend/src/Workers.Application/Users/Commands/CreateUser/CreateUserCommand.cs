namespace Workers.Application.Users.Commands.CreateUser;

using MediatR;
using Workers.Application.Users.DTOs;
using Workers.Domain.Enums;


public record CreateUserCommand(
    string Email,
    string PhoneNumber,
    string? FirstName,
    string? LastName,
    UserRole Role
) : IRequest<UserDto>;
