using Workers.Domain.Entities.Users;

namespace Workers.Application.Users;

public interface IUserRepository
{
    Task<bool> EmailExistsAsync(
        string email, 
        CancellationToken cancellationToken = default);
    
    Task<bool> PhoneExistsAsync(
        string phone, 
        CancellationToken cancellationToken = default);
    
    Task AddAsync(
        User user, 
        CancellationToken cancellationToken = default);
    
    Task<User?> GetByIdAsync(
        Guid id, 
        CancellationToken cancellationToken = default);
    
    Task<List<User>> GetPagedAsync(
        int page, 
        int pageSize, 
        CancellationToken cancellationToken = default);
}
