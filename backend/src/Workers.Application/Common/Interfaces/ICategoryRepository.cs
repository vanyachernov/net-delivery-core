using Workers.Application.Categories.DTOs;
using Workers.Domain.Entities.Categories;

namespace Workers.Application.Common.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetDirectChildrenAsync(Guid? parentId, bool overpassIsDeleteFilter, CancellationToken ct);
    Task<List<Category>> GetAllAsync(bool overpassIsDeleteFilter, CancellationToken ct);

    Task<bool> SlugExistsAsync(string slug, Guid? excludeId, CancellationToken ct);
    Task<bool> HasChildrenAsync(Guid id, CancellationToken ct);

    Task AddAsync(Category category, CancellationToken ct);
    void Update(Category category);
    Task<Category?> GetByIdAsync(Guid id, bool overpassIsDeleteFilter, CancellationToken ct);
    void SoftDelete(Category category);
    Task<bool> ExistsAsync(Guid requestParentId, CancellationToken ct);
}
