using MediatR;

namespace Workers.Application.Categories.Commands.DeleteCagtegory;

public record DeleteCategoryCommand(Guid Id) : IRequest;