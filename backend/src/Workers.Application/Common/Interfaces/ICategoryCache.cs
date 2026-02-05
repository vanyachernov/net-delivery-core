using Workers.Application.Categories.DTOs;
using Workers.Application.Categories.Queries.GetCategories;

namespace Workers.Application.Common.Interfaces;

public interface ICategoryCache
{
    Task<List<CategoryDto>?> GetAsync(Guid? parentId, CategoryLoadMode mode, bool overpassIsDeleteFilter, CancellationToken ct);
    Task SetAsync(Guid? parentId, CategoryLoadMode mode, bool overpassIsDeleteFilter, List<CategoryDto> data, CancellationToken ct);
    Task InvalidateAsync(CancellationToken ct);
}
