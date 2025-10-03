using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Resources;
using FluentValidation;

namespace Daylog.Application.Users.Validators;

public sealed class CreateUserRequestDtoValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserRequestDtoValidator()
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
    }
}
