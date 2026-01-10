using Daylog.Application.Abstractions.Dtos;
using Daylog.Domain.UserProfiles;

namespace Daylog.Application.Users.Dtos.Request;

public sealed class GetUsersOffsetPaginationRequestDto : OffsetPaginationRequestDtoBase, IRequestDto
{
    public string? Name { get; init; }

    public string? Email { get; init; }

    public UserProfileEnum? ProfileId { get; init; }
}
