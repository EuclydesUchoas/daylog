using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Extensions;
using Daylog.Application.Users.Services.Contracts;
using Daylog.Shared.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Services;

public sealed class GetUserByIdService(
    IAppDbContext appDbContext
    ) : IGetUserByIdService
{
    public async Task<Result<UserResponseDto?>> HandleAsync(GetUserByIdRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<UserResponseDto?>(ResultError.NullData);
        }

        if (requestDto.Id == Guid.Empty)
        {
            return Result.Success<UserResponseDto?>(null);
        }

        var userDto = await appDbContext.Users.AsNoTracking()
            .Where(x => x.Id == requestDto.Id)
            .SelectUserResponseDto()
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(userDto);
    }
}
