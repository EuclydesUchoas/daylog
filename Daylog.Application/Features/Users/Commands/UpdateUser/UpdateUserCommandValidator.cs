using Daylog.Application.Resources;
using FluentValidation;

namespace Daylog.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
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
            .GreaterThan(0)
            .WithMessage(AppMessages.User_ProfileIsRequired)
            .IsInEnum()
            .WithMessage(AppMessages.User_ProfileIsInvalid);
    }
}
