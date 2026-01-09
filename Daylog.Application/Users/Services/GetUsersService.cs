using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Common.Mappings;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Extensions;
using Daylog.Application.Users.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Services;

public sealed class GetUsersService(
    IAppDbContext appDbContext
    ) : IGetUsersService
{
    public async Task<Result<ICollectionResponseDto<UserResponseDto>>> HandleAsync(GetUsersRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<ICollectionResponseDto<UserResponseDto>>(ResultError.NullData);
        }

        var usersDtos = await appDbContext.Users.AsNoTracking()
            .SelectUserResponseDto()
            .ToListAsync(cancellationToken);

        var responseDto = usersDtos.ToCollectionResponseDto();

        return Result.Success(responseDto);
    }
}
