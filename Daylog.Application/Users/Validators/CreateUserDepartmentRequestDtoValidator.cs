using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Resources;
using FluentValidation;

namespace Daylog.Application.Users.Validators;

public sealed class CreateUserDepartmentRequestDtoValidator : AbstractValidator<CreateUserDepartmentRequestDto>
{
    public CreateUserDepartmentRequestDtoValidator()
    {
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage(UserMessages.DepartmentIdIsRequired);
    }
}
