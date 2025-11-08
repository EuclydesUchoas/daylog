using Daylog.Application.Common.Resources;
using Daylog.Application.Users.Dtos.Request;
using FluentValidation;

namespace Daylog.Application.Users.Validators;

public sealed class DeleteUserRequestDtoValidator : AbstractValidator<DeleteUserRequestDto>
{
    public DeleteUserRequestDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(AppMessages.User_IdIsRequired);
    }
}
