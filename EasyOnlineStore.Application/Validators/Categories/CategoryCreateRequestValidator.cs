using FluentValidation;
using EasyOnlineStore.Application.DTOs.Requests.Category;

namespace EasyOnlineStore.Application.Validators.Categories;

public class CategoryCreateRequestValidator : AbstractValidator<CategoryCreateRequest>
{
    public CategoryCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name cannot be empty")
            .MinimumLength(3).WithMessage("Category name must be at least 3 characters long")
            .MaximumLength(50).WithMessage("Category name must be between 3 and 50 characters");
    }
}