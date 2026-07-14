using FluentValidation;
using EasyOnlineStore.Application.DTOs.Requests.Order;

namespace EasyOnlineStore.Application.Validators.Orders;

public class OrderItemCreateRequestValidator : AbstractValidator<OrderItemCreateRequest>
{
    public OrderItemCreateRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("Product ID cannot be empty");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0");
    }
}