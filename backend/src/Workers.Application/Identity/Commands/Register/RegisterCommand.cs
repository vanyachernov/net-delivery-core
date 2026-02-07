using MediatR;
using Workers.Application.Common.Models;

namespace Workers.Application.Identity.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Role = "Client"
) : IRequest<AuthenticationResult>;
