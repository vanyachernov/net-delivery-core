using MediatR;
using Workers.Application.Categories.DTOs;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Constants;
using Workers.Domain.Entities.Categories;
using Workers.Domain.Exceptions;

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
            throw new ConflictException("Slug already exists.", ErrorCodes.Category.DuplicateSlug);
        }

        if (request.ParentId is not null && !await repo.ExistsAsync(request.ParentId.Value, cancellationToken))
        {
            throw new BadRequestException("Parent category not found.");
        }

        var category = new Category
        {
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
            category.IsDeleted,
            SubCategories: null
        );
    }
}
