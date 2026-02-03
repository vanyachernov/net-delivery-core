using Microsoft.EntityFrameworkCore;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Entities.Users;
using Workers.Infrastructure.Persistence;

namespace Workers.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db) => _db = db;

    public Task<bool> EmailExistsAsync(string email, CancellationToken ct) =>
        _db.Users.AnyAsync(x => x.Email == email, ct);

    public Task<bool> PhoneExistsAsync(string phone, CancellationToken ct) =>
        _db.Users.AnyAsync(x => x.PhoneNumber == phone, ct);

    public async Task AddAsync(User user, CancellationToken ct) =>
        await _db.Users.AddAsync(user, ct);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _db.Users.FirstOrDefaultAsync(x => x.Id == id, ct);

    public Task<List<User>> GetPagedAsync(int page, int pageSize, CancellationToken ct) =>
        _db.Users
            .OrderByDescending(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

    public void Remove(User user) => _db.Users.Remove(user);
}