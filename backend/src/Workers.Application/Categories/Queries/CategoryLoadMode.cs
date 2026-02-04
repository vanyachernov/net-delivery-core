using MediatR;
using Workers.Application.Categories.DTOs;

namespace Workers.Application.Categories.Queries;
public enum CategoryLoadMode { Direct, All }

public record GetCategoriesQuery(Guid? ParentId, CategoryLoadMode Mode) : IRequest<List<CategoryDto>>;
