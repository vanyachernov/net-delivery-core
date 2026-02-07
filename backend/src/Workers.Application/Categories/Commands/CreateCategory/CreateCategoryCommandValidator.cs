using System.Text.RegularExpressions;
using FluentValidation;

namespace Workers.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    // private static readonly Regex SlugRegex = new("^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.IgnoreCase);

    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters");

        // RuleFor(x => x.Slug)
        //     .NotEmpty()
        //     .WithMessage("Slug is required")
        //     .MaximumLength(150)
        //     .WithMessage("Slug must not exceed 150 characters")
        //     .Matches(SlugRegex)
        //     .WithMessage("Slug must contain only letters, numbers, and hyphens");
    }
}
