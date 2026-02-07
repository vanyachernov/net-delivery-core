using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Workers.Application.Common.Models;
using Workers.Application.Identity;
using Workers.Application.Identity.DTOs;
using Workers.Domain.Entities.Users;

namespace Workers.Infrastructure.Identity;

public class IdentityService(
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    ITokenService tokenService,
    IConfiguration configuration) : IIdentityService
{
    public async Task<AuthenticationResult> RegisterAsync(RegisterUserDto dto)
    {
        var roleName = dto.Role.ToString();
        
        var user = new User
        {
            Email = dto.Email,
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = dto.Role
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
        
        var token = tokenService.GenerateToken(user, [roleName]);
        var refreshToken = tokenService.GenerateRefreshToken();
        
        user.RefreshToken = refreshToken;
        var refreshTokenExpiryDays = int.Parse(configuration["JwtSettings:RefreshTokenExpiryDays"] ?? "7");
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);
        await userManager.UpdateAsync(user);
        
        return AuthenticationResult.Success(token, refreshToken, user.Id.ToString(), user.UserName, roleName);
    }

    public async Task<AuthenticationResult> CreateUserAsync(CreateUserDto dto)
    {
        var roleName = dto.Role.ToString();
        
        var user = new User
        {
            Email = dto.Email,
            UserName = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.PhoneNumber,
            AvatarUrl = dto.AvatarUrl,
            Role = dto.Role
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
        
        await userManager.AddToRoleAsync(user, roleName);

        return AuthenticationResult.Success(null, null, user.Id.ToString(), user.UserName, roleName);
    }

    public async Task<AuthenticationResult> LoginAsync(LoginUserDto dto)
    {
        var user = await userManager.FindByEmailAsync(dto.Email);
        
        if (user == null)
        {
            return AuthenticationResult.Failure("Invalid email or password.");
        }

        if (!await userManager.CheckPasswordAsync(user, dto.Password))
        {
            return AuthenticationResult.Failure("Invalid email or password.");
        }
        
        var roles = await userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Client";
        
        var token = tokenService.GenerateToken(user, roles);
        var refreshToken = tokenService.GenerateRefreshToken();
        
        user.RefreshToken = refreshToken;
        var refreshTokenExpiryDays = int.Parse(configuration["JwtSettings:RefreshTokenExpiryDays"] ?? "7");
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);
        await userManager.UpdateAsync(user);

        var userName = user.UserName ?? user.Email ?? "Unknown";

        return AuthenticationResult.Success(
            token, 
            refreshToken,
            user.Id.ToString(), 
            userName, 
            role);
    }

    public async Task<AuthenticationResult> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null)
        {
            return AuthenticationResult.Failure("Invalid access token");
        }

        var userId = principal.FindFirst("uid")?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            return AuthenticationResult.Failure("Invalid token claims");
        }

        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return AuthenticationResult.Failure("User not found");
        }

        if (user.RefreshToken != request.RefreshToken)
        {
            return AuthenticationResult.Failure("Invalid refresh token");
        }

        if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return AuthenticationResult.Failure("Refresh token expired");
        }

        var roles = await userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Client";

        var newAccessToken = tokenService.GenerateToken(user, roles);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        var refreshTokenExpiryDays = int.Parse(configuration["JwtSettings:RefreshTokenExpiryDays"] ?? "7");
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(refreshTokenExpiryDays);
        await userManager.UpdateAsync(user);

        var userName = user.UserName ?? user.Email ?? "Unknown";

        return AuthenticationResult.Success(
            newAccessToken,
            newRefreshToken,
            user.Id.ToString(),
            userName,
            role);
    }
}
