using Workers.Domain.Entities.Categories;

namespace Workers.Application.Common.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetDirectChildrenAsync(Guid? parentId, CancellationToken ct);
    Task<List<Category>> GetAllAsync(CancellationToken ct);

    Task<bool> SlugExistsAsync(string slug, Guid? excludeId, CancellationToken ct);
    Task<bool> HasChildrenAsync(Guid id, CancellationToken ct);

    Task AddAsync(Category category, CancellationToken ct);
    void Update(Category category);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken ct);
    void SoftDelete(Category category);
    Task<bool> ExistsAsync(Guid requestParentId, CancellationToken ct);
}
