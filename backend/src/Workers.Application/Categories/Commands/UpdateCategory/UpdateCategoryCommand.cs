using MediatR;
using Workers.Application.Categories.DTOs;

namespace Workers.Application.Categories.Commands.UpdateCategory;

public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    string? IconUrl,
    Guid? ParentId
) : IRequest<CategoryDto>;