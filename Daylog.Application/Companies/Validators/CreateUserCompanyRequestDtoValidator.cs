using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Extensions;
using Daylog.Application.Companies.Dtos.Request;
using Daylog.Shared.Core.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Companies.Validators;

public sealed class CreateCompanyUserRequestDtoValidator : AbstractValidator<CreateCompanyUserRequestDto>
{
    public CreateCompanyUserRequestDtoValidator(IAppDbContext appDbContext)
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(AppMessages.CompanyUser_UserIsRequired)
            .ExistsUserId(appDbContext)
            .WithMessage(x => string.Format(AppMessages.User_UserNotExists, x.UserId));
    }
}
