using MediatR;
using Workers.Application.Categories.DTOs;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Entities.Categories;

namespace Workers.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(
    ICategoryRepository repo,
    IUnitOfWork uow,
    ICategoryCache cache
) : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(
        CreateCategoryCommand request, 
        CancellationToken cancellationToken = default)
    {
        if (await repo.SlugExistsAsync(request.Slug, excludeId: null, cancellationToken))
        {
            throw new AppValidationException("slug", "Slug already exists.");
        }

        if (request.ParentId is not null && !await repo.ExistsAsync(request.ParentId.Value, cancellationToken))
        {
            throw new AppValidationException("parentId", "Parent category not found.");
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Slug = request.Slug.Trim().ToLowerInvariant(),
            Description = request.Description?.Trim(),
            IconUrl = request.IconUrl?.Trim(),
            ParentId = request.ParentId,
            CreatedAt = DateTime.UtcNow
        };

        await repo.AddAsync(category, cancellationToken);
        await uow.SaveChangesAsync(cancellationToken);

        await cache.InvalidateAsync(cancellationToken);

        return new CategoryDto(
            category.Id,
            category.Name,
            category.Slug,
            category.Description,
            category.IconUrl,
            category.ParentId,
            SubCategories: null
        );
    }
}

public class AppValidationException : Exception
{
    public string Field { get; }
    public string Error { get; }

    public AppValidationException(string field, string error) : base($"{field}: {error}")
    {
        Field = field;
        Error = error;
    }
}
