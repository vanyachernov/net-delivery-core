using MediatR;
using Microsoft.Extensions.Logging;
using Workers.Application.Identity.DTOs;
using Workers.Application.Common.Models;

namespace Workers.Application.Identity.Commands.Login;

public class LoginCommandHandler(
    IIdentityService identityService,
    ILogger<LoginCommandHandler> logger)
    : IRequestHandler<LoginCommand, AuthenticationResult>
{
    public async Task<AuthenticationResult> Handle(
        LoginCommand request, 
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Attempting login for user {Email}", request.Email);
        
        var result = await identityService.LoginAsync(new LoginUserDto(request.Email, request.Password));

        if (result.Succeeded)
        {
            logger.LogInformation("Login successful for user {Email}", request.Email);
        }
        else
        {
            logger.LogWarning("Login failed for user {Email}: {Error}", request.Email, result.Error);
        }

        return result;
    }
}
