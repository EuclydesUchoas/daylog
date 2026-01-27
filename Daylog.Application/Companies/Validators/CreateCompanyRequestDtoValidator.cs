using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Extensions;
using Daylog.Application.Companies.Dtos.Request;
using Daylog.Shared.Core.Resources;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Companies.Validators;

public sealed class CreateCompanyRequestDtoValidator : AbstractValidator<CreateCompanyRequestDto>
{
    private readonly IAppDbContext _appDbContext;

    public CreateCompanyRequestDtoValidator(IValidator<CreateCompanyUserRequestDto> createCompanyUserValidator, IAppDbContext appDbContext)
    {
        _appDbContext = appDbContext;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(AppMessages.Company_NameIsRequired)
            .MaximumLength(255)
            .WithMessage(string.Format(AppMessages.Company_NameLengthTooLong, 255));

        RuleForEach(x => x.Users)
            .SetValidator(createCompanyUserValidator);
    }

    public override ValidationResult Validate(ValidationContext<CreateCompanyRequestDto> context)
    {
        context.AddExistingUsersIdsAsync(GetValidUsersIds(context), _appDbContext).GetAwaiter().GetResult();
        
        return base.Validate(context);
    }

    public override async Task<ValidationResult> ValidateAsync(ValidationContext<CreateCompanyRequestDto> context, CancellationToken cancellation = default)
    {
        await context.AddExistingUsersIdsAsync(GetValidUsersIds(context), _appDbContext, cancellation);

        return await base.ValidateAsync(context, cancellation);
    }

    private static HashSet<Guid> GetValidUsersIds(ValidationContext<CreateCompanyRequestDto> context)
    {
        var createCompany = context.InstanceToValidate;

        return createCompany.Users
            .Select(x => x.UserId)
            .Where(x => x != Guid.Empty)
            .ToHashSet();
    }
}
