using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Mappings;
using Daylog.Application.Users.Services.Contracts;
using Daylog.Domain.Users;
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

        var userId = new UserId(requestDto.Id);

        var userDto = await appDbContext.Users.AsNoTracking()
            .Where(x => x.Id == userId)
            .SelectUserResponseDto()
            .FirstOrDefaultAsync(cancellationToken);

        if (userDto is null)
        {
            return Result.Success<UserResponseDto?>(null);
        }

        return Result.Success<UserResponseDto?>(userDto);
    }
}
