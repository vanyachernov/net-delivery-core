using Microsoft.EntityFrameworkCore;
using Workers.Application.Users;
using Workers.Domain.Entities.Users;
using Workers.Infrastructure.Persistence;

namespace Workers.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
    {
        return await dbContext.Users
            .AsNoTracking()
            .AnyAsync(x => x.Email == email, ct);
    }

    public async Task<bool> PhoneExistsAsync(string phone, CancellationToken ct)
    {
        return await dbContext.Users
            .AsNoTracking()
            .AnyAsync(x => x.PhoneNumber == phone, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct) =>
        await dbContext.Users.AddAsync(user, ct);

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public async Task<List<User>> GetPagedAsync(int page, int pageSize, CancellationToken ct)
    {
        return await dbContext.Users
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(ct);
    }
}