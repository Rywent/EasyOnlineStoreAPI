using FluentValidation;
using EasyOnlineStore.Application.DTOs.Requests.User;

namespace EasyOnlineStore.Application.Validators.Users;

public class UpdateRoleRequestValidator : AbstractValidator<UpdateRoleRequest>
{
    public UpdateRoleRequestValidator()
    {
        RuleFor(m => m.Roles)
            .NotNull().WithMessage("The roles cannot be null.")
            .ForEach(role =>
            {
                role.NotEmpty().WithMessage("The role cannot be empty.");
            });
    }
}