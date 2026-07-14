using FluentValidation;
using EasyOnlineStore.Application.DTOs.Requests.Cart;

namespace EasyOnlineStore.Application.Validators.Carts;

public class CartAddItemRequestValidator : AbstractValidator<CartAddItemRequest>
{
    public CartAddItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID cannot be empty");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}