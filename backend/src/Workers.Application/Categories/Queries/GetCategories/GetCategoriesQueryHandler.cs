using MediatR;
using Microsoft.Extensions.Logging;
using Workers.Application.Categories.DTOs;
using Workers.Application.Categories.Mappers;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Entities.Categories;

namespace Workers.Application.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler(
    ICategoryRepository repo,
    ICategoryCache cache,
    ILogger<GetCategoriesQueryHandler> logger
) : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken ct)
    {
        List<CategoryDto>? cached = null;

        try
        {
            cached = await cache.GetAsync(request.ParentId, request.Mode, request.OverpassIsDeleteFilter, ct);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Category cache get failed");
        }

        if (cached is not null) return cached;

        List<CategoryDto> result;

        if (request.Mode == CategoryLoadMode.Direct)
        {
            var entities = await repo.GetDirectChildrenAsync(
                request.ParentId,
                request.OverpassIsDeleteFilter,
                ct);

            result = entities.Select(CategoryMapper.ToDtoNoChildren).ToList();
        }
        else
        {
            var all = await repo.GetAllAsync(
                request.OverpassIsDeleteFilter,
                ct);

            var lookup = CategoryMapper.BuildLookup(all);

            var rootParentId = request.ParentId;

            var roots = lookup.GetValueOrDefault(rootParentId) ?? [];

            result = roots.Select(x => CategoryMapper.BuildTree(x, lookup)).ToList();
        }

        try
        {
            await cache.SetAsync(request.ParentId, request.Mode, request.OverpassIsDeleteFilter, result, ct);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Category cache set failed");
        }

        return result;
    }

}
