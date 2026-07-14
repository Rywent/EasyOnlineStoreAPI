using FluentValidation;
using EasyOnlineStore.Application.DTOs.Requests.Product;

namespace EasyOnlineStore.Application.Validators.Products;

public class ProductCreateRequestValidator : AbstractValidator<ProductCreateRequest>
{
    public ProductCreateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty")
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long")
            .MaximumLength(100).WithMessage("Name must be between 2 and 100 characters");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty")
            .MinimumLength(15).WithMessage("Description must be at least 15 characters long")
            .MaximumLength(1000).WithMessage("Description must be between 15 and 1000 characters");
        
        RuleFor(x => x.ShortDescription)
            .MinimumLength(10).WithMessage("ShortDescription must be at least 10 characters long")
            .MaximumLength(50).WithMessage("ShortDescription must be between 10 and 50 characters")
            .When(x => !string.IsNullOrEmpty(x.ShortDescription));
        
        RuleFor(x => x.Stock)
            .GreaterThan(0).WithMessage("Stock must be greater than 0");
        
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0")
            .LessThan(10000000).WithMessage("Price must be less than $10.000.000");
        
        RuleFor(x => x.OldPrice)
            .GreaterThan(0).WithMessage("OldPrice must be greater than 0")
            .LessThan(10000000).WithMessage("OldPrice must be less than $10.000.000")
            .When(x => x.OldPrice != null);

        RuleFor(x => x.ImageUrls)
            .NotEmpty().WithMessage("ImageUrls cannot be empty")
            .Must(list => list.Count <= 15).WithMessage("You cannot upload more than 15 images.");
        
        RuleForEach(x => x.ImageUrls).ChildRules(img =>
        {
            img.RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("ImageUrl cannot be empty");
        });

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("The product must have a category.");
        
        RuleFor(x => x.WarehouseId)
            .NotEmpty().WithMessage("The product must have a warehouse.");


    }
    
}