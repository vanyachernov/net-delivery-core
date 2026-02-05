using MediatR;
using Workers.Application.Categories.DTOs;
using Workers.Application.Categories.Queries.GetCategories;

namespace Workers.Application.Categories.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id, CategoryLoadMode Mode = CategoryLoadMode.Direct)
    : IRequest<CategoryDto?>;
