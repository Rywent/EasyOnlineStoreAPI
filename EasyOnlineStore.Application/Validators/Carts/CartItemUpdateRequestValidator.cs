using FluentValidation;
using EasyOnlineStore.Application.DTOs.Requests.Cart;

namespace EasyOnlineStore.Application.Validators.Carts;

public class CartItemUpdateRequestValidator : AbstractValidator<CartItemUpdateRequest>
{
    public CartItemUpdateRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID cannot be empty");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}