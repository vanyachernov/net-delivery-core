using MediatR;
using Workers.Application.Categories.DTOs;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Entities.Categories;

namespace Workers.Application.Categories.Queries;

public record GetCategoriesList(Guid? ParentId, CategoryLoadMode Mode = CategoryLoadMode.Direct)
    : IRequest<List<CategoryDto>>;

public class GetCategoriesListHandler(
    ICategoryRepository repo,
    ICategoryCache cache
) : IRequestHandler<GetCategoriesList, List<CategoryDto>>
{
    public async Task<List<CategoryDto>> Handle(GetCategoriesList request, CancellationToken ct)
    {
        var cached = await cache.GetAsync(request.ParentId, request.Mode, ct);
        if (cached is not null) return cached;

        List<CategoryDto> result;

        if (request.Mode == CategoryLoadMode.Direct)
        {
            var entities = await repo.GetDirectChildrenAsync(request.ParentId, ct);
            result = entities.Select(ToDtoNoChildren).ToList();
        }
        else
        {
            var all = await repo.GetAllAsync(ct);

            var lookup = all.GroupBy(x => x.ParentId)
                .ToDictionary(g => g.Key, g => g.ToList());

            var roots = lookup.TryGetValue(request.ParentId, out var rootList)
                ? rootList
                : new List<Category>();

            result = roots.Select(x => BuildTree(x, lookup)).ToList();
        }

        await cache.SetAsync(request.ParentId, request.Mode, result, ct);
        return result;
    }

    private static CategoryDto ToDtoNoChildren(Category c) =>
        new(c.Id, c.Name, c.Slug, c.Description, c.IconUrl, c.ParentId, null);

    private static CategoryDto BuildTree(Category c, Dictionary<Guid?, List<Category>> lookup)
    {
        var children = lookup.TryGetValue(c.Id, out var list) ? list : new List<Category>();
        return new CategoryDto(
            c.Id, c.Name, c.Slug, c.Description, c.IconUrl, c.ParentId,
            children.Select(ch => BuildTree(ch, lookup)).ToList()
        );
    }
}
