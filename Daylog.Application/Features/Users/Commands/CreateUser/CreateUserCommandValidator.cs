using Daylog.Application.Resources;
using Daylog.Domain.Entities.Users;
using FluentValidation;

namespace Daylog.Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IValidator<CreateUserDepartmentCommand> createUserDepartmentCommandValidator)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(AppMessages.User_NameIsRequired);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(AppMessages.User_EmailIsRequired)
            .EmailAddress()
            .WithMessage(AppMessages.User_EmailIsInvalid);

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(AppMessages.User_PasswordIsRequired)
            .MinimumLength(8)
            .WithMessage(AppMessages.User_PasswordMustBeAtLeast8CharactersLong);

        RuleFor(x => x.Profile)
            .GreaterThan(0)
            .WithMessage(AppMessages.User_ProfileIsRequired)
            .Must(x => Enum.IsDefined(typeof(UserProfileEnum), x))
            .WithMessage(AppMessages.User_ProfileIsInvalid);

        RuleFor(x => x.UserDepartments)
            .NotEmpty()
            .WithMessage(AppMessages.User_DepartmentsAreRequired)
            .ForEach(x => x.SetValidator(createUserDepartmentCommandValidator));
    }
}
