using Daylog.Application.Resources;
using FluentValidation;

namespace Daylog.Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserDepartmentCommandValidator : AbstractValidator<CreateUserDepartmentCommand>
{
    public CreateUserDepartmentCommandValidator()
    {
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage(AppMessages.UserDepartment_DepartmentIdIsRequired);
    }
}
