using Daylog.Application.Abstractions.Data;
using Daylog.Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Features.Users.Queries.GetUsers;

public sealed class GetUsersQueryHandler(
    IAppDbContext appDbContext
    ) : IRequestHandler<GetUsersQuery, IEnumerable<User>>
{
    public async Task<IEnumerable<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await appDbContext.Users.AsNoTracking()
            .ToListAsync(cancellationToken);

        return users;
    }
}
