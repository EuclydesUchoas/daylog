using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Common.Mappings;
using Daylog.Application.Common.Results;
using Daylog.Application.Companies.Dtos.Request;
using Daylog.Application.Companies.Dtos.Response;
using Daylog.Application.Companies.Extensions;
using Daylog.Application.Companies.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Companies.Services;

public sealed class GetCompaniesService(
    IAppDbContext appDbContext
    ) : IGetCompaniesService
{
    public async Task<Result<ICollectionResponseDto<CompanyResponseDto>>> HandleAsync(GetCompaniesRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<ICollectionResponseDto<CompanyResponseDto>>(ResultError.NullData);
        }

        var companiesDtos = await appDbContext.Companies.AsNoTracking()
            .SelectCompanyResponseDto()
            .ToListAsync(cancellationToken);

        var responseDto = companiesDtos.ToCollectionResponseDto();

        return Result.Success(responseDto);
    }
}
