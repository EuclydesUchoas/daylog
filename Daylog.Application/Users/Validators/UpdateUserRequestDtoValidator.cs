using Daylog.Application.Common.Resources;
using Daylog.Application.Users.Dtos.Request;
using FluentValidation;

namespace Daylog.Application.Users.Validators;

public sealed class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserRequestDtoValidator()
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
    }
}
