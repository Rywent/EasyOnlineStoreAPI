using FluentValidation;
using EasyOnlineStore.Application.DTOs.Requests.User;

namespace EasyOnlineStore.Application.Validators.Users;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().EmailAddress();
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password cannot be empty");
    }
}