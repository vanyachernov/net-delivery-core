using FluentValidation;

namespace Workers.Application.Categories.Queries.GetCategories;

public class GetCategoriesQueryValidator : AbstractValidator<GetCategoriesQuery>
{
    public GetCategoriesQueryValidator()
    {
        RuleFor(x => x.Mode)
            .IsInEnum()
            .WithMessage("Invalid category load mode");

        RuleFor(x => x.ParentId)
            .NotEmpty()
            .When(x => x.ParentId.HasValue)
            .WithMessage("Parent category ID cannot be empty");
    }
}
