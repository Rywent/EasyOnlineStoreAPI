using FluentValidation;
using EasyOnlineStore.Application.DTOs.Requests.Product;

namespace EasyOnlineStore.Application.Validators.Products;

public class ProductImageUploadRequestValidator : AbstractValidator<ProductImageUploadRequest>
{
    public ProductImageUploadRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Image ID cannot be empty.");

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID cannot be empty.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("Image URL cannot be empty.");
    }
}