using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Dtos.Users.Response;

public sealed record UserResponseDto(
    int Id,
    string Name,
    string Email,
    string Password,
    UserProfileEnum Profile,
    ICollection<UserDepartmentResponseDto> UserDepartments,
    DateTime CreatedAt,
    int? CreatedByUserId,
    DateTime UpdatedAt,
    int? UpdatedByUserId,
    bool IsDeleted,
    DateTime? DeletedAt,
    int? DeletedByUserId
    ) : IResponseDto;
