using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Common.Mappings;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Extensions;
using Daylog.Application.Users.Services.Contracts;
using Daylog.Shared.Data.Extensions;
using Daylog.Shared.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Services;

public sealed class GetUsersKeysetPaginationService(
    IAppDbContext appDbContext
    ) : IGetUsersKeysetPaginationService
{
    public async Task<Result<IKeysetPaginationResponseDto<UserResponseDto, Guid>>> HandleAsync(GetUsersKeysetPaginationRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<IKeysetPaginationResponseDto<UserResponseDto, Guid>>(ResultError.NullData);
        }

        var paginationOptions = new KeysetPaginationOptions<UserResponseDto, Guid>
        {
            PageSize = requestDto.PageSize!.Value,
            IdentitySelectorExpression = x => x.Id,
            LastIdentity = requestDto.LastIdentity,
            OrderByExpression = requestDto.SortBy.HasValue && Enum.IsDefined(requestDto.SortBy.Value) ? x => EF.Property<object>(x, requestDto.SortBy.Value.ToString()) : null,
            OrderByDescending = requestDto.SortDirection!.Value,
        };

        var paginationResult = await appDbContext.Users.AsNoTracking()
            .Search(x => x.Name, requestDto.Name)
            .Search(x => x.Email, requestDto.Email)
            .Search(x => x.UserProfileId, requestDto.ProfileId)
            .SelectUserResponseDto()
            .KeysetPaginationAsync(paginationOptions);

        var users = paginationResult.ToPagedResponseDto();

        return Result.Success(users);
    }
}
