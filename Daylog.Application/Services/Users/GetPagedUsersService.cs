using Daylog.Application.Abstractions.Data;
using Daylog.Application.Abstractions.Services.Users;
using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Models;
using Daylog.Domain.Entities.Users;
using Daylog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Services.Users;

public sealed class GetPagedUsersService(
    IAppDbContext appDbContext
    ) : IGetPagedUsersService
{
    public async Task<PagedResponseModel<User>> HandleAsync(GetPagedUsersRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null || !requestDto.HasPagedOptions)
            return PagedResponseModel<User>.EmptyFromPagedRequestDto(requestDto);

        if (requestDto.IncludeTotalItems ?? false)
        {
            var queryBase = appDbContext.Users.AsNoTracking()
                .WhereIf(x => x.Name.Contains(requestDto.Name!), !string.IsNullOrWhiteSpace(requestDto.Name))
                .WhereIf(x => x.Email.Contains(requestDto.Email!), !string.IsNullOrWhiteSpace(requestDto.Email))
                .WhereIf(x => x.Profile == requestDto.Profile, requestDto.Profile.HasValue)
                .OrderBy(x => x.Id);

            var queryPaged = queryBase
                .ApplyPaging(requestDto.PageNumber!.Value, requestDto.PageSize!.Value);

            var pagedUsersWithTotal = await queryBase
                .GroupBy(x => 1)
                .Select(x => new PagedResponseModel<User>(
                    requestDto.PageNumber!.Value,
                    requestDto.PageSize!.Value,
                    queryPaged.ToList(),
                    x.Count()
                    ))
                .FirstOrDefaultAsync(cancellationToken) ?? PagedResponseModel<User>.EmptyFromPagedRequestDto(requestDto);

            return pagedUsersWithTotal;
        }

        var users = await appDbContext.Users.AsNoTracking()
            .OrderBy(x => x.Id)
            .ApplyPaging(requestDto.PageNumber!.Value, requestDto.PageSize!.Value)
            .ToListAsync(cancellationToken);

        var pagedUsers = new PagedResponseModel<User>(
            requestDto.PageNumber!.Value,
            requestDto.PageSize!.Value,
            users,
            null
            );

        return pagedUsers;
    }
}
