using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Common.Mappings;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Mappings;
using Daylog.Application.Users.Services.Contracts;
using Daylog.Domain.Users;
using Daylog.Shared.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

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

        /*var test1 = appDbContext.Users.AsNoTracking()
            .SelectTest1()
            .ToList();*/

        /*var test2 = appDbContext.Users.AsNoTracking()
            .Search(x => x.Name, requestDto.Name)
            .Search(x => x.Email, requestDto.Email)
            .Search(x => x.Profile, requestDto.Profile);

        var test3 = test2
            *//*.Select(x => new
            {
                User = x,
                Total = test2.Count()
            })*//*
            .Skip(0)
            .Take(10)
            .SelectMany(x => x)
            .ToList();*/

        var test2 = appDbContext.Users.AsNoTracking()
            .Search(x => x.Name, requestDto.Name)
            .Search(x => x.Email, requestDto.Email)
            .Search(x => x.Profile, requestDto.Profile);

        Guid? lastId = null;
        //Guid lastId = Guid.Parse("019b430b-3d48-7918-a8ec-8df3f7618da2");

        var test3 = test2
            .OrderBy(x => x.Id)
            .WhereIf(x => x.Id > lastId, lastId.HasValue)
            .Take(requestDto.PageSize!.Value + 1)
            .SelectUserResponseDto()
            .ToList();

        bool hasMore = test3.Count > requestDto.PageSize!.Value;
        if (hasMore)
        {
            test3.RemoveAt(test3.Count - 1);
        }

        var test3_1 = new KeySetPaginationResult<UserResponseDto>
        {
            Items = test3,
            LastId = test3.Count > 0 ? test3[^1].Id : null,
            HasMore = hasMore
        };

        /*var test4 = test2
            .OrderBy(x => x.Id)
            .WhereIf(x => x.Id > lastId, lastId.HasValue)
            .Take(requestDto.PageSize!.Value + 1)
            //.GroupBy(x => 1)
            .Select(chunk => new
            {
                Items = chunk.ToList(),
                //LastId = chunk.Select(x => x.Id).Last(),
                HasMore = chunk.ElementAtOrDefault(requestDto.PageSize!.Value) != null,
            })
            .FirstOrDefault();*/

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

public static class GetPagedUsersServiceExtensions
{
    private static readonly Expression<Func<Domain.Users.User, Test1>> ToTest1Expression = user => new Test1
    {
        Name = user.Name,
        Email = user.Email
    };

    private static readonly Func<Domain.Users.User, Test1> ToTest1Func = ToTest1Expression.Compile();

    public static Test1 ToTest1(this Domain.Users.User user)
        => ToTest1Func(user);


    public static IQueryable<Test1> SelectTest1(this IQueryable<Domain.Users.User> queryable)
        => queryable.Select(ToTest1Expression);
}

public sealed class Test1
{
    public required string Name { get; init; }

    public required string Email { get; init; }
}

public sealed class KeySetPaginationResult<TItems>
{
    public required IEnumerable<TItems> Items { get; init; }

    public required Guid? LastId { get; init; }

    public required bool HasMore { get; init; }
}
