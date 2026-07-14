using FluentValidation;
using EasyOnlineStore.Application.DTOs.Requests.Warehouse;

namespace EasyOnlineStore.Application.Validators.Warehouses;

public class WarehouseUpdateRequestValidator : AbstractValidator<WarehouseUpdateRequest>
{
    public WarehouseUpdateRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Warehouse name cannot be empty")
            .MinimumLength(2).WithMessage("Warehouse name must be at least 2 characters long")
            .MaximumLength(100).WithMessage("Warehouse name must be between 2 and 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Name));

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location cannot be empty")
            .MinimumLength(2).WithMessage("Location must be at least 2 characters long")
            .MaximumLength(100).WithMessage("Location must be between 2 and 100 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Location));

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address cannot be empty")
            .MinimumLength(5).WithMessage("Address must be at least 5 characters long")
            .MaximumLength(200).WithMessage("Address must be between 5 and 200 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone number cannot be empty")
            .MinimumLength(7).WithMessage("Phone number must be at least 7 characters long")
            .MaximumLength(20).WithMessage("Phone number must be less than 20 characters")
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.DeliveryCost)
            .GreaterThanOrEqualTo(0).WithMessage("Delivery cost cannot be negative")
            .When(x => x.DeliveryCost.HasValue);
    }
}