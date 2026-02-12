using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Extensions;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Shared.Core.Resources;
using FluentValidation;

namespace Daylog.Application.Users.Validators;

public sealed class CreateUserCompanyRequestDtoValidator : AbstractValidator<CreateUserCompanyRequestDto>
{
    public CreateUserCompanyRequestDtoValidator(IAppDbContext appDbContext)
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage(AppMessages.UserCompany_CompanyIsRequired)
            .ExistsCompanyId(appDbContext)
            .WithMessage(x => string.Format(AppMessages.Company_CompanyNotExists, x.CompanyId));
    }
}
