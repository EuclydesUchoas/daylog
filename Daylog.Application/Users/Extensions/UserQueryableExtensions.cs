using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Domain;
using Daylog.Domain.UserProfiles;
using Daylog.Domain.Users;
using Daylog.Shared.Core.Constants;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Daylog.Application.Users.Extensions;

public static class UserQueryableExtensions
{
    public static IQueryable<UserResponseDto> SelectUserResponseDto(this IQueryable<User> users)
    {
        string culture = Cultures.CurrentCultureSnakeCase;

        return users.Select(x => new UserResponseDto
        {
            Id = x.Id,
            Name = x.Name,
            Email = x.Email,
            UserProfileId = x.UserProfileId,
            UserProfileName = EF.Property<string>(x.UserProfile, $"{nameof(UserProfile.Name)}_{culture}"),
            UserCompanies = x.UserCompanies.Select(x2 => new UserCompanyResponseDto
            {
                UserId = x2.UserId,
                UserName = x2.User.Name,
                CompanyId = x2.CompanyId,
                CompanyName = x2.Company.Name,
                CreatedInfo = new CreatedInfoResponseDto
                {
                    CreatedAt = x2.CreatedAt,
                    CreatedByUserId = x2.CreatedByUserId,
                    CreatedByUserName = x2.CreatedByUser!.Name
                },
                UpdatedInfo = new UpdatedInfoResponseDto
                {
                    UpdatedAt = x2.UpdatedAt,
                    UpdatedByUserId = x2.UpdatedByUserId,
                    UpdatedByUserName = x2.UpdatedByUser!.Name
                },
                DeletedInfo = new DeletedInfoResponseDto
                {
                    IsDeleted = x2.IsDeleted,
                    DeletedAt = x2.DeletedAt,
                    DeletedByUserId = x2.DeletedByUserId,
                    DeletedByUserName = x2.DeletedByUser!.Name,
                }
            }),
            CreatedInfo = new CreatedInfoResponseDto
            {
                CreatedAt = x.CreatedAt,
                CreatedByUserId = x.CreatedByUserId,
                CreatedByUserName = x.CreatedByUser!.Name
            },
            UpdatedInfo = new UpdatedInfoResponseDto
            {
                UpdatedAt = x.UpdatedAt,
                UpdatedByUserId = x.UpdatedByUserId,
                UpdatedByUserName = x.UpdatedByUser!.Name
            },
            DeletedInfo = new DeletedInfoResponseDto
            {
                IsDeleted = x.IsDeleted,
                DeletedAt = x.DeletedAt,
                DeletedByUserId = x.DeletedByUserId,
                DeletedByUserName = x.DeletedByUser!.Name,
            }
        });
    }
}

public static class AuditExpressions
{
    public static Expression<Func<ICreatable, CreatedInfoResponseDto>> CreatedInfo =
        x => new CreatedInfoResponseDto
        {
            CreatedAt = x.CreatedAt,
            CreatedByUserId = x.CreatedByUserId,
            CreatedByUserName = x.CreatedByUser!.Name
        };
}
