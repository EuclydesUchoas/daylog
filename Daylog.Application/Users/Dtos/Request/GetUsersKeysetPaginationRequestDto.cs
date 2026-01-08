using Daylog.Application.Abstractions.Dtos;
using Daylog.Domain.Users;

namespace Daylog.Application.Users.Dtos.Request;

public sealed class GetUsersKeysetPaginationRequestDto<TIdentity> : KeysetPaginationRequestDtoBase<TIdentity>, IRequestDto
    where TIdentity : struct, IComparable<TIdentity>, IEquatable<TIdentity>
{
    public string? Name { get; init; }

    public string? Email { get; init; }

    public UserProfileEnum? Profile { get; init; }
}
