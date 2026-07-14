using FluentValidation;
using EasyOnlineStore.Application.DTOs.Requests.User;

namespace EasyOnlineStore.Application.Validators.Users;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("The first name cannot be empty.")
            .MinimumLength(2).WithMessage("The first name must be at least 2 characters.")
            .MaximumLength(25).WithMessage("The first name must be less than 25 characters.");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("The last name cannot be empty.")
            .MinimumLength(2).WithMessage("The last name must be at least 2 characters.")
            .MaximumLength(25).WithMessage("The last name must be less than 25 characters.");

        RuleFor(x => x.Roles)
            .NotNull().WithMessage("The roles cannot be null.")
            .ForEach(role =>
            {
                role.NotEmpty().WithMessage("The role cannot be empty.");
            });
    }
}