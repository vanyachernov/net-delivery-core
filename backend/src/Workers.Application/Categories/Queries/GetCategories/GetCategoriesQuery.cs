using MediatR;
using Workers.Application.Categories.DTOs;

namespace Workers.Application.Categories.Queries.GetCategories;

public record GetCategoriesQuery(Guid? ParentId, CategoryLoadMode Mode = CategoryLoadMode.Direct)
    : IRequest<List<CategoryDto>>;
