using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Resources.Users;
using FluentValidation;

namespace Daylog.Application.Validators.Users;

public sealed class CreateUserDepartmentRequestDtoValidator : AbstractValidator<CreateUserDepartmentRequestDto>
{
    public CreateUserDepartmentRequestDtoValidator()
    {
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage(UserMessages.DepartmentIdIsRequired);
    }
}
