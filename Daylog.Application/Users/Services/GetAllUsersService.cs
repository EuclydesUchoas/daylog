using Daylog.Application.Abstractions.Data;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Application.Users.Services.Contracts;
using Daylog.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Users.Services;

public sealed class GetAllUsersService(
    IAppDbContext appDbContext
    ) : IGetAllUsersService
{
    public async Task<IEnumerable<User>> HandleAsync(GetAllUsersRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
            return [];

        var users = await appDbContext.Users.AsNoTracking()
            .Include(x => x.UserDepartments)
                .ThenInclude(x => x.Department)
            .ToListAsync(cancellationToken);

        return users;
    }
}
