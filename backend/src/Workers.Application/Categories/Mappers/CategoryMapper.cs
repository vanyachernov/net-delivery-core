using Workers.Application.Categories.DTOs;
using Workers.Domain.Entities.Categories;

namespace Workers.Application.Categories.Mappers;

public static class CategoryMapper
{
    public static CategoryDto ToDtoNoChildren(Category category) =>
        new(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.IconUrl,
            category.ParentId,
            category.IsDeleted,
            SubCategories: null
        );

    public static CategoryDto BuildTree(Category category, Dictionary<Guid?, List<Category>> lookup)
    {
        var children = lookup.GetValueOrDefault(category.Id) ?? [];

        return new CategoryDto(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.IconUrl,
            category.ParentId,
            category.IsDeleted,
            SubCategories: children.Select(child => BuildTree(child, lookup)).ToList()
        );
    }

    public static Dictionary<Guid?, List<Category>> BuildLookup(List<Category> categories) =>
        categories.GroupBy(x => x.ParentId)
            .ToDictionary(g => g.Key, g => g.ToList());
}
