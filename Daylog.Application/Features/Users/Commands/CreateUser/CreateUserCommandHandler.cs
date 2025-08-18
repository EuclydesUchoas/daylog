using Daylog.Application.Abstractions.Data;
using Daylog.Application.Mappings.Users;
using Daylog.Domain.Entities.Users;
using FluentValidation;
using MediatR;

namespace Daylog.Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserCommandHandler(
    IValidator<CreateUserCommand> _validator,
    IAppDbContext _appDbContext
    ) : IRequestHandler<CreateUserCommand, User>
{
    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = request.ToDomain();
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        _appDbContext.Users.Add(user);

        await _appDbContext.SaveChangesAsync(cancellationToken);

        return user;
    }
}
