using Daylog.Application.Abstractions.Dtos;
using Daylog.Domain.Users;

namespace Daylog.Application.Users.Dtos.Response;

public sealed record UserResponseDto(
    Guid Id,
    string Name,
    string Email,
    UserProfileEnum Profile,
    DateTime CreatedAt,
    Guid? CreatedByUserId,
    DateTime UpdatedAt,
    Guid? UpdatedByUserId,
    bool IsDeleted,
    DateTime? DeletedAt,
    Guid? DeletedByUserId
    ) : IResponseDto;
