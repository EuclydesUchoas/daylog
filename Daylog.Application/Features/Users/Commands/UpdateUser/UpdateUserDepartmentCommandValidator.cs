using Daylog.Application.Resources;
using FluentValidation;

namespace Daylog.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserDepartmentCommandValidator : AbstractValidator<UpdateUserDepartmentCommand>
{
    public UpdateUserDepartmentCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage(AppMessages.UserDepartment_DepartmentIdIsRequired);
    }
}
