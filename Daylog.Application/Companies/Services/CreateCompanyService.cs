using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Results;
using Daylog.Application.Companies.Dtos.Request;
using Daylog.Application.Companies.Dtos.Response;
using Daylog.Application.Companies.Extensions;
using Daylog.Application.Companies.Services.Contracts;
using FluentValidation;

namespace Daylog.Application.Companies.Services;

public sealed class CreateCompanyService(
    IValidator<CreateCompanyRequestDto> validator,
    IAppDbContext appDbContext
    ) : ICreateCompanyService
{
    public async Task<Result<CompanyResponseDto>> HandleAsync(CreateCompanyRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<CompanyResponseDto>(ResultError.NullData);
        }

        var validationResult = await validator.ValidateAsync(requestDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Failure<CompanyResponseDto>(ResultError.Validation(validationResult.Errors));
        }

        var company = requestDto.ToCompany();
        ArgumentNullException.ThrowIfNull(company, nameof(company));

        appDbContext.Companies.Add(company);

        await appDbContext.SaveChangesAsync(cancellationToken);

        var responseDto = company.ToCompanyResponseDto()!;

        return Result.Success(responseDto);
    }
}
