namespace Workers.Application.Categories.DTOs;

public record CategoryDto(
    Guid Id,
    string Name,
    string Slug,
    string? Description,
    string? IconUrl,
    Guid? ParentId,
    bool IsDeleted,
    List<CategoryDto>? SubCategories
);
