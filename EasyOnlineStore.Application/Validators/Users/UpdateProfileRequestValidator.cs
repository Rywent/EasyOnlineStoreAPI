using FluentValidation;
using EasyOnlineStore.Application.DTOs.Requests.User;

namespace EasyOnlineStore.Application.Validators.Users;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .MinimumLength(2).WithMessage("The first name must be at least 2 characters.")
            .MaximumLength(25).WithMessage("The first name must be less than 25 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.FirstName));
        
        RuleFor(x => x.LastName)
            .MinimumLength(2).WithMessage("The last name must be at least 2 characters.")
            .MaximumLength(25).WithMessage("The last name must be less than 25 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.LastName));

        RuleFor(x => x.Country)
            .MinimumLength(2).WithMessage("The country must be at least 2 characters.")
            .MaximumLength(58).WithMessage("The country must be less than 58 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Country));
        
        RuleFor(x => x.City)
            .MinimumLength(2).WithMessage("The city must be at least 2 characters.")
            .MaximumLength(100).WithMessage("The city must be less than 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.City));
        
        RuleFor(x => x.Address)
            .MinimumLength(2).WithMessage("The address must be at least 2 characters.")
            .MaximumLength(100).WithMessage("The address must be less than 100 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.Address));
    }
}