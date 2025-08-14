using Daylog.Application.Abstractions.Data;
using Daylog.Application.Mappings.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler(
    IAppDbContext _appDbContext
    ) : IRequestHandler<DeleteUserCommand, bool>
{
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.ToDomain();
        ArgumentNullException.ThrowIfNull(user, nameof(user));

        bool userExists = await _appDbContext.Users.AsNoTracking()
            .AnyAsync(u => u.Id == user.Id, cancellationToken);

        if (!userExists)
        {
            return false;
        }

        _appDbContext.Users.Remove(user);

        int deletedUsers = await _appDbContext.SaveChangesAsync(cancellationToken);

        return deletedUsers > 0;
    }
}
