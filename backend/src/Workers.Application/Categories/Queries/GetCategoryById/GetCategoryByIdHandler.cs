using MediatR;
using Workers.Application.Categories.DTOs;
using Workers.Application.Categories.Mappers;
using Workers.Application.Categories.Queries.GetCategories;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Entities.Categories;

namespace Workers.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdHandler(ICategoryRepository repo)
    : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken ct)
    {
        if (request.Mode == CategoryLoadMode.Direct)
        {
            var entity = await repo.GetByIdAsync(
                request.Id,
                request.OverpassIsDeleteFilter,
                ct);

            return entity is null ? null : CategoryMapper.ToDtoNoChildren(entity);
        }

        var all = await repo.GetAllAsync(
            request.OverpassIsDeleteFilter,
            ct);
        var root = all.FirstOrDefault(x => x.Id == request.Id);
        if (root is null) return null;

        var lookup = CategoryMapper.BuildLookup(all);

        return CategoryMapper.BuildTree(root, lookup);
    }
}
 
