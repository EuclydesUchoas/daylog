using Daylog.Application.Abstractions.Data;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Shared.Core.Resources;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Validators;

public sealed class CreateUserRequestDtoValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserRequestDtoValidator(IValidator<CreateUserCompanyRequestDto> createUserCompanyValidator, IAppDbContext appDbContext)
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(AppMessages.User_NameIsRequired)
            .MaximumLength(255)
            .WithMessage(string.Format(AppMessages.User_NameLengthTooLong, 255));

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(AppMessages.User_EmailIsRequired)
            .EmailAddress()
            .WithMessage(AppMessages.User_EmailIsInvalid)
            .MaximumLength(255)
            .WithMessage(string.Format(AppMessages.User_EmailLengthTooLong, 255));

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(AppMessages.User_PasswordIsRequired)
            .MinimumLength(8)
            .WithMessage(string.Format(AppMessages.User_PasswordLengthTooShort, 8))
            .MaximumLength(255)
            .WithMessage(string.Format(AppMessages.User_PasswordLengthTooLong, 255));

        RuleFor(x => x.ProfileId)
            .NotEmpty()
            .WithMessage(AppMessages.User_ProfileIsRequired)
            .IsInEnum()
            .WithMessage(AppMessages.User_ProfileIsInvalid);

        Guid[]? validCompaniesIds = null;

        RuleForEach(x => x.UserCompanies)
            .SetValidator(createUserCompanyValidator)
            .MustAsync(async (createUser, createUserCompany, cancellationToken) =>
            {
                validCompaniesIds ??= await appDbContext.Companies.AsNoTracking()
                    .Where(x => x.UserCompanies
                        .Select(x2 => x2.CompanyId)
                        .Contains(x.Id))
                    .Select(x => x.Id)
                    .ToArrayAsync(cancellationToken);
                return validCompaniesIds.Contains(createUserCompany.CompanyId);
            })
            .WithMessage((x, x2) => string.Format(AppMessages.Company_IdNotExist, x2.CompanyId));
    }
}
