using MediatR;
using Workers.Application.Categories.DTOs;

namespace Workers.Application.Categories.Queries.GetCategories;

public record GetCategoriesQuery(
    Guid? ParentId,
    CategoryLoadMode Mode = CategoryLoadMode.Direct,
    bool OverpassIsDeleteFilter = false)
    : IRequest<List<CategoryDto>>;
