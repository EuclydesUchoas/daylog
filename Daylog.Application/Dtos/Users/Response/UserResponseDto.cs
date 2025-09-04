using Daylog.Application.Abstractions.Dtos;
using Daylog.Domain.Entities.Users;

namespace Daylog.Application.Dtos.Users.Response;

public sealed record UserResponseDto(
    Guid Id,
    string Name,
    string Email,
    UserProfileEnum Profile,
    ICollection<UserDepartmentResponseDto> UserDepartments,
    DateTime CreatedAt,
    Guid? CreatedByUserId,
    DateTime UpdatedAt,
    Guid? UpdatedByUserId,
    bool IsDeleted,
    DateTime? DeletedAt,
    Guid? DeletedByUserId
    ) : IResponseDto;
