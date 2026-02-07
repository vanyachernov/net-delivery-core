using MediatR;
using Workers.Application.Common.Models;
using Workers.Domain.Enums;

namespace Workers.Application.Identity.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    UserRole Role = UserRole.Client
) : IRequest<AuthenticationResult>;
