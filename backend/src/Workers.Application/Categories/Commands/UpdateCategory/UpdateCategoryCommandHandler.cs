


using MediatR;
using Workers.Application.Categories.DTOs;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Entities.Categories;
using Workers.Domain.Exceptions;

namespace Workers.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(
    ICategoryRepository repo,
    IUnitOfWork uow,
    ICategoryCache cache
) : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken ct)
    {
        var entity = await repo.GetByIdAsync(request.Id, ct) 
            ?? throw new NotFoundException(nameof(Category), request.Id);

        entity.Name = request.Name.Trim();
        entity.Slug = request.Slug.Trim().ToLowerInvariant();
        entity.Description = request.Description?.Trim();
        entity.IconUrl = request.IconUrl?.Trim();
        entity.ParentId = request.ParentId;
        entity.UpdatedAt = DateTime.UtcNow;

        repo.Update(entity);
        await uow.SaveChangesAsync(ct);

        await cache.InvalidateAsync(ct);

        return new CategoryDto(
            entity.Id,
            entity.Name,
            entity.Slug,
            entity.Description,
            entity.IconUrl,
            entity.ParentId,
            SubCategories: null
        );
    }
}
