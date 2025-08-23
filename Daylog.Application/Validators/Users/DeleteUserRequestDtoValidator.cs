using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Resources;
using FluentValidation;

namespace Daylog.Application.Validators.Users;

public sealed class DeleteUserRequestDtoValidator : AbstractValidator<DeleteUserRequestDto>
{
    public DeleteUserRequestDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(AppMessages.User_IdIsRequired);
    }
}
