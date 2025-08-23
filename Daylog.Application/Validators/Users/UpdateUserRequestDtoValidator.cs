using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Resources;
using FluentValidation;

namespace Daylog.Application.Validators.Users;

public sealed class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserRequestDtoValidator(IValidator<UpdateUserDepartmentRequestDto> updateUserDepartmentRequestDtoValidator)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(AppMessages.User_IdIsRequired);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(AppMessages.User_NameIsRequired);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(AppMessages.User_EmailIsRequired)
            .EmailAddress()
            .WithMessage(AppMessages.User_EmailIsInvalid);

        RuleFor(x => x.Profile)
            .NotEmpty()
            .WithMessage(AppMessages.User_ProfileIsRequired)
            .IsInEnum()
            .WithMessage(AppMessages.User_ProfileIsInvalid);

        RuleFor(x => x.UserDepartments)
            .NotEmpty()
            .WithMessage(AppMessages.User_DepartmentsAreRequired)
            .ForEach(x => x.SetValidator(updateUserDepartmentRequestDtoValidator));
    }
}
