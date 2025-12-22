using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Common.Mappings;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Mappings;
using Daylog.Application.Users.Services.Contracts;
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

        var queryBase = appDbContext.Users.AsNoTracking()
            .Search(x => x.Name, requestDto.Name)
            .Search(x => x.Email, requestDto.Email)
            .Search(x => x.Profile, requestDto.Profile);

        if (requestDto.IncludeTotalItems ?? false)
        {
            var usersWithTotal = await queryBase
                .PaginateWithTotal(requestDto.PageNumber!.Value, requestDto.PageSize!.Value, x => x.Id, x => x.SelectUserResponseDto())
                .FirstOrDefaultAsync(cancellationToken);

            var pagedUsersWithTotal = usersWithTotal.ToPagedResponseDto(requestDto);

            return Result.Success(pagedUsersWithTotal);
        }

        var users = await queryBase
            .Paginate(requestDto.PageNumber!.Value, requestDto.PageSize!.Value, x => x.Id, x => x.SelectUserResponseDto())
            .ToListAsync(cancellationToken);

        var pagedUsers = users.ToPagedResponseDto(requestDto);

        return Result.Success(pagedUsers);
    }
}
