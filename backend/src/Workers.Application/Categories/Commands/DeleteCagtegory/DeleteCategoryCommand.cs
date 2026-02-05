using MediatR;

namespace Workers.Application.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : IRequest;