using MediatR;
using Microsoft.Extensions.Logging;
using Workers.Application.Categories.DTOs;
using Workers.Application.Common.Interfaces;
using Workers.Application.Users.DTOs;
using Workers.Application.Users.Queries.GetUserById;
using Workers.Domain.Entities.Categories;

namespace Workers.Application.Categories.Queries;

public class GetCategoryByIdHandler(ICategoryRepository repo)
    : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken ct)
    {
        if (request.Mode == CategoryLoadMode.Direct)
        {
            var entity = await repo.GetByIdAsync(request.Id, ct);
            return entity is null ? null : ToDtoNoChildren(entity);
        }

        var all = await repo.GetAllAsync(ct);
        var root = all.FirstOrDefault(x => x.Id == request.Id);
        if (root is null) return null;

        var lookup = all.GroupBy(x => x.ParentId)
            .ToDictionary(g => g.Key, g => g.ToList());

        return BuildTree(root, lookup);
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