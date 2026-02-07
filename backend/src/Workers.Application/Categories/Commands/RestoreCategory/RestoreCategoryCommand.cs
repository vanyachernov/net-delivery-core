using MediatR;

namespace Workers.Application.Categories.Commands.RestoreCategory;

public record RestoreCategoryCommand(Guid Id) : IRequest;
