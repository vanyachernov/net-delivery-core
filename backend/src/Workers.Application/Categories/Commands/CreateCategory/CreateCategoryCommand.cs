using MediatR;
using Workers.Application.Categories.DTOs;

namespace Workers.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    string Slug,
    string? Description,
    string? IconUrl,
    Guid? ParentId
) : IRequest<CategoryDto>;
