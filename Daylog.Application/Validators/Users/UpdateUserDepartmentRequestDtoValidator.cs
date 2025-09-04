using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Resources.Users;
using FluentValidation;

namespace Daylog.Application.Validators.Users;

public sealed class UpdateUserDepartmentRequestDtoValidator : AbstractValidator<UpdateUserDepartmentRequestDto>
{
    public UpdateUserDepartmentRequestDtoValidator()
    {
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage(UserMessages.DepartmentIdIsRequired);
    }
}
