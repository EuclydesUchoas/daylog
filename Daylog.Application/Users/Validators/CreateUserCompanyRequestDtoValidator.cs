using Daylog.Shared.Core.Resources;
using Daylog.Application.Users.Dtos.Request;
using FluentValidation;

namespace Daylog.Application.Users.Validators;

public sealed class CreateUserCompanyRequestDtoValidator : AbstractValidator<CreateUserCompanyRequestDto>
{
    public CreateUserCompanyRequestDtoValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage(AppMessages.UserCompany_CompanyIdIsRequired);
    }
}
