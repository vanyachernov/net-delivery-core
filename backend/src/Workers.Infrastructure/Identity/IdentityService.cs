using Microsoft.AspNetCore.Identity;
using Workers.Application.Common.Interfaces;
using Workers.Application.Common.Models;

namespace Workers.Infrastructure.Identity;

public class IdentityService(
    UserManager<ApplicationIdentityUser> userManager,
    RoleManager<IdentityRole> roleManager) : IIdentityService
{
    public async Task<(string IdentityId, string Error)> CreateUserAsync(CreateIdentityUserDto userDto)
    {
        var user = new ApplicationIdentityUser
        {
            Id = userDto.UserId.ToString(),
            UserName = userDto.Email,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
        };

        var result = await userManager.CreateAsync(user, userDto.Password);

        if (!result.Succeeded)
        {
            return (string.Empty, string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        if (!await roleManager.RoleExistsAsync(userDto.RoleName))
        {
            await roleManager.CreateAsync(new IdentityRole(userDto.RoleName));
        }

        var roleResult = await userManager.AddToRoleAsync(user, userDto.RoleName);
        
        if (!roleResult.Succeeded)
        {
            await userManager.DeleteAsync(user);
            return (string.Empty, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
        }

        return (user.Id, string.Empty);
    }
}
