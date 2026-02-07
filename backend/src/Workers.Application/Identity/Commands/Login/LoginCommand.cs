using MediatR;
using Workers.Application.Common.Models;

namespace Workers.Application.Identity.Commands.Login;

public record LoginCommand(
    string Email, 
    string Password) 
    : IRequest<AuthenticationResult>;
