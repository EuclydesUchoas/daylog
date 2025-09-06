using Daylog.Application.Abstractions.Data;
using Daylog.Application.Shared.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Services.Contracts;
using Daylog.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Services;

public sealed class GetUserByIdService(
    IAppDbContext appDbContext
    ) : IGetUserByIdService
{
    public async Task<Result<User?>> HandleAsync(GetUserByIdRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<User?>(ResultError.NullData);
        }

        if (requestDto.Id == Guid.Empty)
        {
            return Result.Success<User?>(null);
        }

        var userId = new UserId(requestDto.Id);

        var user = await appDbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
        {
            return Result.Success<User?>(null);
        }

        return Result.Success<User?>(user);
    }
}
