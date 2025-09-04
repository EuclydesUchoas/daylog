using Daylog.Application.Abstractions.Data;
using Daylog.Application.Abstractions.Services.Users;
using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Mappings.Users;
using Daylog.Application.Results;
using Daylog.Application.Results.Users;
using Daylog.Domain.Entities.Users;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Services.Users;

public sealed class CreateUserService(
    IValidator<CreateUserRequestDto> validator,
    IAppDbContext appDbContext
    ) : ICreateUserService
{
    public async Task<Result<User>> HandleAsync(CreateUserRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<User>(ResultError.NullData);
        }

        var validationResult = await validator.ValidateAsync(requestDto, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Result.Failure<User>(ResultError.Validation(validationResult.Errors));
        }

        bool emailIsInUse = await appDbContext.Users.AsNoTracking()
            .AnyAsync(x => x.Email.ToLower() == requestDto.Email.ToLower(), cancellationToken);

        if (emailIsInUse)
        {
            return Result.Failure<User>(UserResultErrors.EmailNotUnique);
        }

        var user = requestDto.ToDomain();
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        appDbContext.Users.Add(user);

        await appDbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(user);
    }
}
