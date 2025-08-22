using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Resources;
using FluentValidation;

namespace Daylog.Application.Validators.Users;

public sealed class CreateUserDepartmentRequestDtoValidator : AbstractValidator<CreateUserDepartmentRequestDto>
{
    public CreateUserDepartmentRequestDtoValidator()
    {
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage(AppMessages.UserDepartment_DepartmentIdIsRequired);
    }
}
