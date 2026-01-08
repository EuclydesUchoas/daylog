using Daylog.Application.Abstractions.Data;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Dtos.Response;
using Daylog.Application.Users.Extensions;
using Daylog.Application.Users.Results;
using Daylog.Application.Users.Services.Contracts;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Services;

public sealed class UpdateUserService(
    IValidator<UpdateUserRequestDto> validator,
    IAppDbContext appDbContext
    ) : IUpdateUserService
{
    public async Task<Result<UserResponseDto>> HandleAsync(UpdateUserRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<UserResponseDto>(ResultError.NullData);
        }

        var validationResult = await validator.ValidateAsync(requestDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Failure<UserResponseDto>(ResultError.Validation(validationResult.Errors));
        }

        bool emailIsInUse = await appDbContext.Users.AsNoTracking()
            .AnyAsync(x => x.Email.Trim().ToLower() == requestDto.Email.Trim().ToLower(), cancellationToken);

        if (emailIsInUse)
        {
            return Result.Failure<UserResponseDto>(UserResultErrors.EmailNotUnique);
        }

        var user = requestDto.ToUser();
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var userDb = await appDbContext.Users // Change Tracking is required for updates
            .FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);
            //?? throw new KeyNotFoundException($"User with ID {user.Id} not found.");

        if (userDb is null)
        {
            return Result.Failure<UserResponseDto>(UserResultErrors.NotFound(user.Id));
        }

        userDb.Update(user);

        await appDbContext.SaveChangesAsync(cancellationToken);

        var userDto = userDb.ToUserResponseDto()!;

        return Result.Success(userDto);
    }
}
