using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Mappings;
using Daylog.Application.Users.Services.Contracts;
using Daylog.Domain.Users;
using Daylog.Shared.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Services;

public sealed class GetPagedUsersService(
    IAppDbContext appDbContext
    ) : IGetPagedUsersService
{
    public async Task<Result<IPagedResponseDto<UserResponseDto>>> HandleAsync(GetPagedUsersRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<IPagedResponseDto<UserResponseDto>>(ResultError.NullData);
        }

        var watch = System.Diagnostics.Stopwatch.StartNew();

        var queryBase = appDbContext.Users.AsNoTracking()
            .Search(x => x.Name, requestDto.Name)
            .Search(x => x.Email, requestDto.Email)
            .Search(x => x.Profile, requestDto.Profile)
            .OrderBy(x => x.Id)
            .SelectUserResponseDto();

        watch.Stop();
        var elapsedMs = watch.Elapsed.TotalMilliseconds;

        var queryPaged = queryBase
            .Paginate(requestDto.PageNumber!.Value, requestDto.PageSize!.Value);

        if (requestDto.IncludeTotalItems ?? false)
        {
            var pagedUsersWithTotal = await queryBase
                .GroupBy(x => 1)
                .Select(x => IPagedResponseDto<UserResponseDto>.FromItems(
                    requestDto.PageNumber!.Value,
                    requestDto.PageSize!.Value,
                    //queryPaged.Select(x2 => x2.ToDto()!),
                    queryPaged.ToList(),
                    x.Count()
                    ))
                .FirstOrDefaultAsync(cancellationToken) ?? IPagedResponseDto<UserResponseDto>.Empty(requestDto);

            return Result.Success(pagedUsersWithTotal);
        }

        var usersDtos = await queryPaged
            //.Select(x => x.ToDto()!)
            .ToListAsync(cancellationToken);

        var pagedUsers = IPagedResponseDto<UserResponseDto>.FromItems(
            requestDto.PageNumber!.Value,
            requestDto.PageSize!.Value,
            usersDtos,
            null
            );

        return Result.Success(pagedUsers);
    }
}
