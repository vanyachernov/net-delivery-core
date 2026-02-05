using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Entities.Users;
using Workers.Infrastructure.Persistence;
using Workers.Infrastructure.Identity;
using Workers.Domain.Enums;

namespace Workers.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
    {
        return await dbContext.Set<ApplicationIdentityUser>()
            .AsNoTracking()
            .AnyAsync(x => x.Email == email, ct);
    }

    public async Task<bool> PhoneExistsAsync(string phone, CancellationToken ct)
    {
        return await dbContext.Set<ApplicationIdentityUser>()
            .AsNoTracking()
            .AnyAsync(x => x.PhoneNumber == phone, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct) =>
        await dbContext.Users.AddAsync(user, ct);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (user == null) return null;

        await EnrichUserWithIdentityAsync(user, ct);
        return user;
    }

    public async Task<List<User>> GetPagedAsync(int page, int pageSize, CancellationToken ct)
    {
        var users = await dbContext.Users
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(ct);

        if (users.Count == 0) return users;

         // Enrich in parallel or bulk
         var userIds = users.Select(u => u.Id.ToString()).ToList();
         
         var identities = await dbContext.Set<ApplicationIdentityUser>()
             .Where(u => userIds.Contains(u.Id))
             .AsNoTracking()
             .ToDictionaryAsync(u => u.Id, u => u, ct);
         
         // Fetch roles
         var userRolesQuery = from ur in dbContext.UserRoles
                              join r in dbContext.Roles on ur.RoleId equals r.Id
                              where userIds.Contains(ur.UserId)
                              select new { ur.UserId, r.Name };
         
         var userRoles = await userRolesQuery.ToListAsync(ct);
         var roleMap = userRoles.GroupBy(x => x.UserId).ToDictionary(g => g.Key, g => g.First().Name); // Take first role

         foreach (var user in users)
         {
             var key = user.Id.ToString();
             if (identities.TryGetValue(key, out var identity))
             {
                 user.Email = identity.Email ?? string.Empty;
                 user.PhoneNumber = identity.PhoneNumber ?? string.Empty;
                 user.IsEmailVerified = identity.EmailConfirmed;
                 user.IsPhoneVerified = identity.PhoneNumberConfirmed;
             }

             if (roleMap.TryGetValue(key, out var roleName) && Enum.TryParse<UserRole>(roleName, out var roleEnum))
             {
                 user.Role = roleEnum;
             }
         }

        return users;
    }

    private async Task EnrichUserWithIdentityAsync(User user, CancellationToken ct)
    {
        var userIdStr = user.Id.ToString();
        var identity = await dbContext.Set<ApplicationIdentityUser>()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userIdStr, ct);

        if (identity != null)
        {
            user.Email = identity.Email ?? string.Empty;
            user.PhoneNumber = identity.PhoneNumber ?? string.Empty;
            user.IsEmailVerified = identity.EmailConfirmed;
            user.IsPhoneVerified = identity.PhoneNumberConfirmed;

            // Get role
            var roleId = await dbContext.UserRoles
                .Where(ur => ur.UserId == userIdStr)
                .Select(ur => ur.RoleId)
                .FirstOrDefaultAsync(ct);

            if (roleId != null)
            {
                var roleName = await dbContext.Roles
                    .Where(r => r.Id == roleId)
                    .Select(r => r.Name)
                    .FirstOrDefaultAsync(ct);

                if (Enum.TryParse<UserRole>(roleName, out var roleEnum))
                {
                    user.Role = roleEnum;
                }
            }
        }
    }
}