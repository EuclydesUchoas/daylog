using Daylog.Application.Abstractions.Data;
using Daylog.Application.Abstractions.Services.Users;
using Daylog.Application.Dtos.Users.Request;
using Daylog.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Services.Users;

public sealed class GetUserByIdService(
    IAppDbContext appDbContext
    ) : IGetUserByIdService
{
    public async Task<User?> HandleAsync(GetUserByIdRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null || requestDto.Id == Guid.Empty)
            return null;

        var userId = new UserId(requestDto.Id);

        var user = await appDbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        return user;
    }
}
