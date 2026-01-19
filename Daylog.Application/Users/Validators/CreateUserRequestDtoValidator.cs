using Daylog.Shared.Core.Resources;
using Daylog.Application.Users.Dtos.Request;
using FluentValidation;

namespace Daylog.Application.Users.Validators;

public sealed class CreateUserRequestDtoValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(AppMessages.User_NameIsRequired)
            .MaximumLength(255)
            .WithMessage(string.Format(AppMessages.User_NameLengthTooLong, 255));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(AppMessages.User_EmailIsRequired)
            .EmailAddress()
            .WithMessage(AppMessages.User_EmailIsInvalid)
            .MaximumLength(255)
            .WithMessage(string.Format(AppMessages.User_EmailLengthTooLong, 255));

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(AppMessages.User_PasswordIsRequired)
            .MinimumLength(8)
            .WithMessage(string.Format(AppMessages.User_PasswordLengthTooShort, 8))
            .MaximumLength(255)
            .WithMessage(string.Format(AppMessages.User_PasswordLengthTooLong, 255));

        RuleFor(x => x.ProfileId)
            .NotEmpty()
            .WithMessage(AppMessages.User_ProfileIsRequired)
            .IsInEnum()
            .WithMessage(AppMessages.User_ProfileIsInvalid);
    }
}
