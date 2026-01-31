using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Users.Enums;
using Daylog.Domain.UserProfiles;

namespace Daylog.Application.Users.Dtos.Request;

public sealed class GetUsersKeysetPaginationRequestDto : KeysetPaginationRequestDtoBase<Guid>, IRequestDto//, IBindableFromHttpContext<GetUsersKeysetPaginationRequestDto>, IParsable<GetUsersKeysetPaginationRequestDto>
{
    public string? Name { get; init; }
    
    public string? Email { get; init; }

    public UserProfileEnum? ProfileId
    {
        get;
        init => field = value.HasValue && Enum.IsDefined(value.Value) ? value : null;
    }

    public UserOrderByEnum? SortBy
    {
        get;
        init => field = value.HasValue && Enum.IsDefined(value.Value) ? value : null;
    }

    /*public static ValueTask<GetUsersKeysetPaginationRequestDto?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        var dto = new GetUsersKeysetPaginationRequestDto
        {
            Name = context.Request.Query["name"],
            Email = context.Request.Query["email"],
            Profile = Enum.TryParse<UserProfileEnum>(context.Request.Query["profile"], true, out var profile) ? profile : null,
            SortBy = Enum.TryParse<UserOrderByEnum>(context.Request.Query["sortBy"], true, out var orderBy) ? orderBy : null,
            LastIdentity = Guid.TryParse(context.Request.Query["lastIdentity"], out var lastIdentity) ? lastIdentity : null,
            PageSize = int.TryParse(context.Request.Query["pageSize"], out var pageSize) ? pageSize : DefaultPageSize,
            SortDirection = Enum.TryParse<OrderByDirectionEnum>(context.Request.Query["sortDirection"], true, out var sortDirection) ? sortDirection : DefaultSortDirection,
        };

        return ValueTask.FromResult(dto)!;
    }

    public static ValueTask<GetUsersKeysetPaginationRequestDto?> BindAsync(HttpContext context)
    {
        var dto = new GetUsersKeysetPaginationRequestDto
        {
            Name = context.Request.Query["name"],
            Email = context.Request.Query["email"],
            Profile = Enum.TryParse<UserProfileEnum>(context.Request.Query["profile"], true, out var profile) ? profile : null,
            SortBy = Enum.TryParse<UserOrderByEnum>(context.Request.Query["sortBy"], true, out var orderBy) ? orderBy : null,
            LastIdentity = Guid.TryParse(context.Request.Query["lastIdentity"], out var lastIdentity) ? lastIdentity : null,
            PageSize = int.TryParse(context.Request.Query["pageSize"], out var pageSize) ? pageSize : DefaultPageSize,
            SortDirection = Enum.TryParse<OrderByDirectionEnum>(context.Request.Query["sortDirection"], true, out var sortDirection) ? sortDirection : DefaultSortDirection,
        };

        return ValueTask.FromResult(dto)!;
    }

    public static GetUsersKeysetPaginationRequestDto Parse(string s, IFormatProvider? provider)
    {
        throw new NotImplementedException();
    }

    public static bool TryParse(string? name, IFormatProvider? provider, out GetUsersKeysetPaginationRequestDto dto)
    {
        dto = null!;
        return true;
    }

    public static bool TryParse(string? name, out GetUsersKeysetPaginationRequestDto dto)
    {
        dto = null!;
        return true;
    }*/
}
