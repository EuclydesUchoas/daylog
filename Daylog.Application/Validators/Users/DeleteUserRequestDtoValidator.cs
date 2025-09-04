using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Resources.Users;
using FluentValidation;

namespace Daylog.Application.Validators.Users;

public sealed class DeleteUserRequestDtoValidator : AbstractValidator<DeleteUserRequestDto>
{
    public DeleteUserRequestDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(UserMessages.IdIsRequired);
    }
}
