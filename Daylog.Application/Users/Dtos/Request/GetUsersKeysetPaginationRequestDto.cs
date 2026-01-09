using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Users.Enums;
using Daylog.Domain.Users;

namespace Daylog.Application.Users.Dtos.Request;

public sealed class GetUsersKeysetPaginationRequestDto<TIdentity> : KeysetPaginationRequestDtoBase<TIdentity>, IRequestDto
    where TIdentity : struct
{
    public string? Name { get; init; }

    public string? Email { get; init; }

    public UserProfileEnum? Profile { get; init; }

    public UserOrderByEnum? OrderBy { get; init; }
}
