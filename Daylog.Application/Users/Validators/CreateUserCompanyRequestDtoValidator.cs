using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Extensions;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Shared.Core.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Validators;

public sealed class CreateUserCompanyRequestDtoValidator : AbstractValidator<CreateUserCompanyRequestDto>
{
    public CreateUserCompanyRequestDtoValidator(IAppDbContext appDbContext)
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty()
            .WithMessage(AppMessages.UserCompany_CompanyIdIsRequired)
            .ExistsCompanyId(appDbContext)
            .WithMessage(x => string.Format(AppMessages.Company_IdNotExist, x.CompanyId));
    }
}
