using Daylog.Domain.Entities.Users;
using System.Text.Json.Serialization;

namespace Daylog.Application.Dtos.Users.Request;

public sealed record UpdateUserRequestDto(
    [property: JsonIgnore] Guid Id, // Id will be set from the route parameter, not from the body
    string Name,
    string Email,
    UserProfileEnum Profile,
    ICollection<UpdateUserDepartmentRequestDto> UserDepartments
    ) : IRequestDto;
