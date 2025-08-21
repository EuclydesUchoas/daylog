using Daylog.Application.Abstractions.Data;
using Daylog.Application.Mappings.Users;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler(
    IValidator<DeleteUserCommand> _validator,
    IAppDbContext _appDbContext
    ) : IRequestHandler<DeleteUserCommand, bool>
{
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var userWithId = request.ToDomain();
        ArgumentNullException.ThrowIfNull(userWithId, nameof(request));

        var user = await _appDbContext.Users
            .Include(x => x.UserDepartments)
            .FirstOrDefaultAsync(u => u.Id == userWithId.Id, cancellationToken);

        if (user is null)
        {
            return false;
        }

        _appDbContext.Users.Remove(user);

        int deletedUsers = await _appDbContext.SaveChangesAsync(cancellationToken);

        return deletedUsers > 0;
    }
}
