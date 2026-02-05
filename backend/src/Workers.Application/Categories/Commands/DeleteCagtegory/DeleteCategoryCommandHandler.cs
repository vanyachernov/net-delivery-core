using MediatR;
using Workers.Application.Categories.Commands.DeleteCategory;
using Workers.Application.Common.Interfaces;


public class DeleteCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork uow,
    ICategoryCache cache
) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
            throw new Exception($"Category '{request.Id}' not found.");

        if (await categoryRepository.HasChildrenAsync(request.Id, cancellationToken))
            throw new Exception("id Cannot delete category that has subcategories.");

        categoryRepository.SoftDelete(entity);
        await uow.SaveChangesAsync(cancellationToken);
        
        await cache.InvalidateAsync(cancellationToken);
    }
}
