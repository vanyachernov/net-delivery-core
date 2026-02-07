using Microsoft.AspNetCore.Identity;
using Workers.Domain.Enums;

namespace Workers.Api.Extensions;

public static class SeedingExtensions
{
    public static async Task SeedRolesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        
        foreach (var role in Enum.GetValues<UserRole>())
        {
            var roleName = role.ToString();
            
            if (role == UserRole.Guest) continue;

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }
        }
    }
}
