using System.Text.RegularExpressions;
using FluentValidation;

namespace Workers.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private static readonly Regex SlugRegex = new("^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.IgnoreCase);

    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Category ID cannot be empty");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("Slug is required")
            .MaximumLength(150)
            .WithMessage("Slug must not exceed 150 characters")
            .Matches(SlugRegex)
            .WithMessage("Slug must contain only letters, numbers, and hyphens");

        RuleFor(x => x.ParentId)
            .NotEmpty()
            .When(x => x.ParentId.HasValue)
            .WithMessage("Parent category ID cannot be empty")
            .NotEqual(x => x.Id)
            .WithMessage("Category cannot be its own parent");
    }
}
