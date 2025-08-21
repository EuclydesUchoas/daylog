using Daylog.Application.Features.Users.Commands.CreateUser;
using Daylog.Application.Resources;
using Daylog.Domain.Entities.Users;
using FluentValidation;

namespace Daylog.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(IValidator<UpdateUserDepartmentCommand> updateUserDepartmentCommandValidator)
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
            .Must(x => Enum.IsDefined(typeof(UserProfileEnum), x))
            .WithMessage(AppMessages.User_ProfileIsInvalid);

        RuleFor(x => x.UserDepartments)
            .NotEmpty()
            .WithMessage(AppMessages.User_DepartmentsAreRequired)
            .ForEach(x => x.SetValidator(updateUserDepartmentCommandValidator));
    }
}
