using Daylog.Application.Abstractions.Data;
using Daylog.Application.Abstractions.Services.Users;
using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Mappings.Users;
using Daylog.Domain.Entities.Users;
using FluentValidation;

namespace Daylog.Application.Services.Users;

public sealed class CreateUserService(
    IValidator<CreateUserRequestDto> validator,
    IAppDbContext appDbContext
    ) : ICreateUserService
{
    public async Task<User> HandleAsync(CreateUserRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        await validator.ValidateAndThrowAsync(requestDto, cancellationToken);

        var user = requestDto.ToDomain();
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        appDbContext.Users.Add(user);

        await appDbContext.SaveChangesAsync(cancellationToken);

        return user;
    }
}
