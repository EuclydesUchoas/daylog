namespace Daylog.Application.Dtos.Users;

public sealed record UserDto(
    int Id,
    string Name,
    string Email,
    string Password,
    int Profile,
    ICollection<UserDepartmentDto> UserDepartments,
    DateTime CreatedAt,
    int? CreatedByUserId,
    DateTime UpdatedAt,
    int? UpdatedByUserId,
    bool IsDeleted,
    DateTime? DeletedAt,
    int? DeletedByUserId
    ) : IDto;
