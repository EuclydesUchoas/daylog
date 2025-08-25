using Daylog.Application.Abstractions.Data;
using Daylog.Application.Abstractions.Services.Users;
using Daylog.Application.Dtos.Users.Request;
using Daylog.Application.Models;
using Daylog.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Services.Users;

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
