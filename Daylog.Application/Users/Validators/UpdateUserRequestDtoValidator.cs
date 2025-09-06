using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Resources;
using FluentValidation;

namespace Daylog.Application.Users.Validators;

public sealed class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserRequestDtoValidator(IValidator<UpdateUserDepartmentRequestDto> updateUserDepartmentRequestDtoValidator)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(UserMessages.IdIsRequired);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(UserMessages.NameIsRequired);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(UserMessages.EmailIsRequired)
            .EmailAddress()
            .WithMessage(UserMessages.EmailIsInvalid);

        RuleFor(x => x.Profile)
            .NotEmpty()
            .WithMessage(UserMessages.ProfileIsRequired)
            .IsInEnum()
            .WithMessage(UserMessages.ProfileIsInvalid);

        RuleFor(x => x.UserDepartments)
            .NotEmpty()
            .WithMessage(UserMessages.DepartmentsAreRequired)
            .ForEach(x => x.SetValidator(updateUserDepartmentRequestDtoValidator));
    }
}
