using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Common.Mappings;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Extensions;
using Daylog.Application.Users.Services.Contracts;
using Daylog.Shared.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Services;

public sealed class GetUsersOffsetPaginationService(
    IAppDbContext appDbContext
    ) : IGetUsersOffsetPaginationService
{
    public async Task<Result<IOffsetPaginationResponseDto<UserResponseDto>>> HandleAsync(GetUsersOffsetPaginationRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<IOffsetPaginationResponseDto<UserResponseDto>>(ResultError.NullData);
        }

        var queryBase = appDbContext.Users.AsNoTracking()
            .Search(x => x.Name, requestDto.Name)
            .Search(x => x.Email, requestDto.Email)
            .Search(x => x.Profile, requestDto.Profile)
            .SelectUserResponseDto();

        if (requestDto.IncludeTotalItems ?? false)
        {
            var usersWithTotal = await queryBase
                .PaginateWithTotal(requestDto.PageNumber!.Value, requestDto.PageSize!.Value, x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);

            var pagedUsersWithTotal = usersWithTotal.ToPagedResponseDto(requestDto);

            return Result.Success(pagedUsersWithTotal);
        }

        var users = await queryBase
            .Paginate(requestDto.PageNumber!.Value, requestDto.PageSize!.Value, x => x.Id)
            .ToListAsync(cancellationToken);

        var pagedUsers = users.ToPagedResponseDto(requestDto);

        return Result.Success(pagedUsers);
    }
}
