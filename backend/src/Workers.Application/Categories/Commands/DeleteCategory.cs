using MediatR;

namespace Workers.Application.Categories.Commands;

public record DeleteCategoryCommand(Guid Id) : IRequest;