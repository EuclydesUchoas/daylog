using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Resources.Users;
using FluentValidation;

namespace Daylog.Application.Validators.Users;

public sealed class CreateUserRequestDtoValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserRequestDtoValidator(IValidator<CreateUserDepartmentRequestDto> createUserDepartmentRequestDtoValidator)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(UserMessages.NameIsRequired);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(UserMessages.EmailIsRequired)
            .EmailAddress()
            .WithMessage(UserMessages.EmailIsInvalid);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(UserMessages.PasswordIsRequired)
            .MinimumLength(8)
            .WithMessage(string.Format(UserMessages.PasswordLengthTooShort, '8'));

        RuleFor(x => x.Profile)
            .NotEmpty()
            .WithMessage(UserMessages.ProfileIsRequired)
            .IsInEnum()
            .WithMessage(UserMessages.ProfileIsInvalid);
        ;
        RuleFor(x => x.UserDepartments)
            .NotEmpty()
            .WithMessage(UserMessages.DepartmentsAreRequired)
            .ForEach(x => x.SetValidator(createUserDepartmentRequestDtoValidator));
    }
}
