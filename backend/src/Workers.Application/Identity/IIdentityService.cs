using Workers.Application.Common.Models;
using Workers.Application.Identity.DTOs;

namespace Workers.Application.Identity;

public interface IIdentityService
{
    Task<AuthenticationResult> RegisterAsync(RegisterUserDto dto);
    
    Task<AuthenticationResult> CreateUserAsync(CreateUserDto dto);

    Task<AuthenticationResult> LoginAsync(LoginUserDto dto);
}
