using Daylog.Application.Abstractions.Dtos;
using Daylog.Domain.UserProfiles;
using System.Text.Json.Serialization;

namespace Daylog.Application.Users.Dtos.Request;

public sealed record UpdateUserRequestDto(
    [property: JsonIgnore] Guid Id, // Id will be set from the route parameter, not from the body
    string Name,
    string Email,
    UserProfileEnum ProfileId
    ) : IRequestDto;
