using MediatR;
using Workers.Application.Categories.Commands.DeleteCategory;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Constants;
using Workers.Domain.Entities.Categories;
using Workers.Domain.Exceptions;


public class DeleteCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork uow,
    ICategoryCache cache
) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await categoryRepository.GetByIdAsync(request.Id, false, cancellationToken);
        if (entity is null)
            throw new NotFoundException(nameof(Category), request.Id);

        if (await categoryRepository.HasChildrenAsync(request.Id, cancellationToken))
            throw new ConflictException("Cannot delete category that has subcategories.", ErrorCodes.Category.HasChildren);

        categoryRepository.SoftDelete(entity);
        await uow.SaveChangesAsync(cancellationToken);
        
        await cache.InvalidateAsync(cancellationToken);
    }
}
