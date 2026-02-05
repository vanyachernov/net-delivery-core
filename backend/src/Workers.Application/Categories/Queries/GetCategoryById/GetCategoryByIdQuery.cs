using MediatR;
using Workers.Application.Categories.DTOs;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Entities.Categories;

namespace Workers.Application.Categories.Queries;

public record GetCategoryByIdQuery(Guid Id, CategoryLoadMode Mode = CategoryLoadMode.Direct)
    : IRequest<CategoryDto?>;
