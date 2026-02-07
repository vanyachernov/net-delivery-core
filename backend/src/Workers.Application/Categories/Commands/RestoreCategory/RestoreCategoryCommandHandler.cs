using MediatR;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Constants;
using Workers.Domain.Entities.Categories;
using Workers.Domain.Exceptions;

namespace Workers.Application.Categories.Commands.RestoreCategory;

public class RestoreCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork,
    ICategoryCache cache
) : IRequestHandler<RestoreCategoryCommand>
{
    public async Task Handle(RestoreCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await categoryRepository.GetByIdAsync(request.Id, true, cancellationToken)
            ?? throw new NotFoundException(nameof(Category), request.Id);

        if (!entity.IsDeleted)
            throw new BadRequestException("Category is not deleted.");

        if (entity.ParentId is not null && !await categoryRepository.ExistsAsync(entity.ParentId.Value, cancellationToken))
            throw new BadRequestException("Parent category not found.");

        if (await categoryRepository.SlugExistsAsync(entity.Slug, entity.Id, cancellationToken))
            throw new ConflictException("Slug already exists.", ErrorCodes.Category.DuplicateSlug);

        entity.IsDeleted = false;
        entity.DeletedAt = null;
        entity.UpdatedAt = DateTime.UtcNow;

        categoryRepository.Update(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cache.InvalidateAsync(cancellationToken);
    }
}
