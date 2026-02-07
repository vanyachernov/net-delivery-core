using Microsoft.AspNetCore.Identity;
using Workers.Application.Common.Models;
using Workers.Application.Identity;
using Workers.Application.Identity.DTOs;
using Workers.Domain.Entities.Users;
using Workers.Domain.Enums;

namespace Workers.Infrastructure.Identity;

public class IdentityService(
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    ITokenService tokenService) : IIdentityService
{
    public async Task<AuthenticationResult> RegisterAsync(RegisterUserDto dto)
    {
        var roleName = string.IsNullOrWhiteSpace(dto.Role) ? "Client" : dto.Role;
        if (!Enum.TryParse<UserRole>(roleName, true, out var roleEnum))
        {
             roleEnum = UserRole.Client;
             roleName = "Client";
        }

        var user = new User
        {
            Email = dto.Email,
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = roleEnum
        };

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return AuthenticationResult.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }

        var roleResult = await userManager.AddToRoleAsync(user, roleName);
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return AuthenticationResult.Failure(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }

        return await LoginAsync(new LoginUserDto(dto.Email, dto.Password));
    }

    public async Task<AuthenticationResult> CreateUserAsync(CreateUserDto dto)
    {
        var user = new User
        {
            Email = dto.Email,
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            AvatarUrl = dto.AvatarUrl
        };
        
        var roleName = string.IsNullOrWhiteSpace(dto.Role) ? "Client" : dto.Role;

        if (Enum.TryParse<UserRole>(roleName, true, out var roleEnum))
        {
             user.Role = roleEnum;
        }
        else
        {
             user.Role = UserRole.Client;
             roleName = "Client";
        }

        var result = await userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded)
        {
            return AuthenticationResult.Failure(string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
        
        await userManager.AddToRoleAsync(user, roleName);

        return AuthenticationResult.Success(null, user.Id.ToString(), user.UserName, roleName);
    }

    public async Task<AuthenticationResult> LoginAsync(LoginUserDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            return AuthenticationResult.Failure("Invalid email or password.");

        if (!await userManager.CheckPasswordAsync(user, dto.Password))
            return AuthenticationResult.Failure("Invalid email or password.");

        var roles = await userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Client";
        var token = tokenService.GenerateToken(user, roles);

        var userName = user.UserName ?? user.Email ?? "Unknown";

        return AuthenticationResult.Success(token, user.Id.ToString(), userName, role);
    }
}
