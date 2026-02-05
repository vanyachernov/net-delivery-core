using Microsoft.EntityFrameworkCore;
using Workers.Application.Categories.DTOs;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Entities.Categories;
using Workers.Infrastructure.Persistence;

namespace Workers.Infrastructure.Repositories;

public class CategoryRepository(ApplicationDbContext db) : ICategoryRepository
{
    public async Task<List<Category>> GetDirectChildrenAsync(Guid? parentId, CancellationToken ct)
    {
        return await db.Set<Category>()
            .AsNoTracking()
            .Where(c => c.ParentId == parentId)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken ct)
    {
        return await db.Set<Category>()
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(ct);
    }

    public Task<Category?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return db.Set<Category>()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
        return db.Set<Category>().AnyAsync(x => x.Id == id, ct);
    }

    public Task<bool> HasChildrenAsync(Guid id, CancellationToken ct)
    {
        return db.Set<Category>().AnyAsync(x => x.ParentId == id, ct);
    }

    public Task<bool> SlugExistsAsync(string slug, Guid? excludeId, CancellationToken ct)
    {
       
        var normalized = slug.Trim().ToLowerInvariant();

        return db.Set<Category>().AnyAsync(x =>
            x.Slug.ToLower() == normalized &&
            (excludeId == null || x.Id != excludeId.Value), ct);
    }

    public Task AddAsync(Category category, CancellationToken ct)
        => db.Set<Category>().AddAsync(category, ct).AsTask();

    public void Update(Category category)
        => db.Set<Category>().Update(category);

    public void SoftDelete(Category category)
    {
        category.IsDeleted = true;
        Update(category);
    }
}
