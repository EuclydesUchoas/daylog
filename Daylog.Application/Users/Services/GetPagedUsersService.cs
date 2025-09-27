using Daylog.Application.Abstractions.Configurations;
using Daylog.Application.Abstractions.Data;
using Daylog.Application.Shared.Mappings;
using Daylog.Application.Shared.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Services.Contracts;
using Daylog.Domain;
using Daylog.Domain.Users;
using Daylog.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Services;

public sealed class GetPagedUsersService(
    IAppDbContext appDbContext,
    IAppConfiguration appConfiguration
    ) : IGetPagedUsersService
{
    public async Task<Result<PagedEntity<User>>> HandleAsync(GetPagedUsersRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
            return Result.Failure<PagedEntity<User>>(ResultError.NullData);

        var databaseProvider = appConfiguration.DatabaseProvider;

        var queryBase = appDbContext.Users.AsNoTracking()
            .Search(x => x.Name, requestDto.Name, databaseProvider)
            .Search(x => x.Email, requestDto.Email, databaseProvider)
            .Search(x => x.Profile, requestDto.Profile, databaseProvider)
            .OrderBy(x => x.Id);

        if (requestDto.IncludeTotalItems ?? false)
        {
            var queryPaged = queryBase
                .Paginate(requestDto.PageNumber!.Value, requestDto.PageSize!.Value);

            var pagedUsersWithTotal = await queryBase
                .GroupBy(x => 1)
                .Select(x => new PagedEntity<User>(
                    requestDto.PageNumber!.Value,
                    requestDto.PageSize!.Value,
                    queryPaged.ToList(),
                    x.Count()
                    ))
                .FirstOrDefaultAsync(cancellationToken) ?? requestDto.ToDomain<User>();

            return Result.Success(pagedUsersWithTotal);
        }

        var users = await queryBase
            .Paginate(requestDto.PageNumber!.Value, requestDto.PageSize!.Value)
            .ToListAsync(cancellationToken);

        var pagedUsers = new PagedEntity<User>(
            requestDto.PageNumber!.Value,
            requestDto.PageSize!.Value,
            users,
            null
            );

        return Result.Success(pagedUsers);
    }
}
