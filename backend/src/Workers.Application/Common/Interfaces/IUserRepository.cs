namespace Workers.Application.Common.Interfaces;

using Workers.Domain.Entities.Users;


public interface IUserRepository
{
    Task<bool> EmailExistsAsync(string email, CancellationToken ct);
    Task<bool> PhoneExistsAsync(string phone, CancellationToken ct);

    Task AddAsync(User user, CancellationToken ct);
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<User>> GetPagedAsync(int page, int pageSize, CancellationToken ct);

    void Remove(User user);
}
