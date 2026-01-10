using Daylog.Application.Users.Dtos.Response;
using Daylog.Domain.Users;

namespace Daylog.Application.Users.Extensions;

public static class UserQueryableExtensions
{
    public static IQueryable<UserResponseDto> SelectUserResponseDto(this IQueryable<User> users)
        => users.Select(x => new UserResponseDto
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email,
            ProfileId = x.ProfileId,
            CreatedAt = x.CreatedAt,
            CreatedByUserId = x.CreatedByUserId,
            UpdatedAt = x.UpdatedAt,
            UpdatedByUserId = x.UpdatedByUserId,
            IsDeleted = x.IsDeleted,
            DeletedAt = x.DeletedAt,
            DeletedByUserId = x.DeletedByUserId
        });
}
