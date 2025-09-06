using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Resources;
using FluentValidation;

namespace Daylog.Application.Users.Validators;

public sealed class DeleteUserRequestDtoValidator : AbstractValidator<DeleteUserRequestDto>
{
    public DeleteUserRequestDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(UserMessages.IdIsRequired);
    }
}
