using Microsoft.EntityFrameworkCore;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Entities.Categories;
using Workers.Infrastructure.Persistence;

namespace Workers.Infrastructure.Repositories;

public class CategoryRepository(ApplicationDbContext db) : ICategoryRepository
{
    private IQueryable<Category> GetCategoriesQuery(bool overpassIsDeleteFilter)
    {
        var query = db.Categories.AsQueryable();
        return overpassIsDeleteFilter ? query.IgnoreQueryFilters() : query;
    }

    public async Task<List<Category>> GetDirectChildrenAsync(Guid? parentId, bool overpassIsDeleteFilter, CancellationToken ct)
    {
        return await GetCategoriesQuery(overpassIsDeleteFilter)
            .AsNoTracking()
            .Where(c => c.ParentId == parentId)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);
    }

    public async Task<List<Category>> GetAllAsync(bool overpassIsDeleteFilter, CancellationToken ct)
    {
        return await GetCategoriesQuery(overpassIsDeleteFilter)
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .ToListAsync(ct);
    }

    public Task<Category?> GetByIdAsync(Guid id, bool overpassIsDeleteFilter, CancellationToken ct)
    {
        return GetCategoriesQuery(overpassIsDeleteFilter)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
        return db.Categories.AnyAsync(x => x.Id == id, ct);
    }

    public Task<bool> HasChildrenAsync(Guid id, CancellationToken ct)
    {
        return db.Categories.AnyAsync(x => x.ParentId == id, ct);
    }

    public Task<bool> SlugExistsAsync(string slug, Guid? excludeId, CancellationToken ct)
    {
       
        var normalized = slug.Trim().ToLowerInvariant();

        return db.Categories.AnyAsync(x =>
            x.Slug.ToLower() == normalized &&
            (excludeId == null || x.Id != excludeId.Value), ct);
    }

    public async Task AddAsync(Category category, CancellationToken ct)
    {
        await db.Categories.AddAsync(category, ct);
    }

    public void Update(Category category)
        => db.Categories.Update(category);

    public void SoftDelete(Category category)
    {
        category.IsDeleted = true;
        Update(category);
    }
}
