using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Extensions;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Shared.Core.Resources;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Validators;

public sealed class CreateUserRequestDtoValidator : AbstractValidator<CreateUserRequestDto>
{
    private readonly IAppDbContext _appDbContext;

    public CreateUserRequestDtoValidator(IValidator<CreateUserCompanyRequestDto> createUserCompanyValidator, IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;

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

        RuleForEach(x => x.Companies)
            .SetValidator(createUserCompanyValidator);
    }

    public override ValidationResult Validate(ValidationContext<CreateUserRequestDto> context)
    {
        context.AddExistingCompaniesIdsAsync(GetCompaniesIds(context), _appDbContext).GetAwaiter().GetResult();
        
        return base.Validate(context);
    }

    public override async Task<ValidationResult> ValidateAsync(ValidationContext<CreateUserRequestDto> context, CancellationToken cancellation = default)
    {
        await context.AddExistingCompaniesIdsAsync(GetCompaniesIds(context), _appDbContext, cancellation);

        return await base.ValidateAsync(context, cancellation);
    }

    private static HashSet<Guid> GetCompaniesIds(ValidationContext<CreateUserRequestDto> context)
    {
        var createUser = context.InstanceToValidate;

        return createUser.Companies
            .Select(x => x.CompanyId)
            .Where(x => x != Guid.Empty)
            .ToHashSet();
    }
}
