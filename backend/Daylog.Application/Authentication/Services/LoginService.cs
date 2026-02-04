using Daylog.Application.Abstractions.Authentication;
using Daylog.Application.Abstractions.Data;
using Daylog.Application.Authentication.Dtos.Request;
using Daylog.Application.Authentication.Dtos.Response;
using Daylog.Application.Authentication.Results;
using Daylog.Application.Authentication.Services.Contracts;
using Daylog.Application.Common.Results;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Authentication.Services;

public sealed class LoginService(
    IAppDbContext appDbContext,
    IPasswordHasher passwordHasher
    ) : ILoginService
{
    public async Task<Result<LoginResponseDto>> HandleAsync(LoginRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<LoginResponseDto>(ResultError.NullData);
        }

        var user = await appDbContext.Users.AsNoTracking()
            .Select(x => new
            {
                x.Id,
                x.Email,
                x.Password
            })
            .FirstOrDefaultAsync(x => x.Email == requestDto.Email, cancellationToken);

        if (user is null || !passwordHasher.Verify(requestDto.Password, user.Password))
        {
            return Result.Failure<LoginResponseDto>(AuthResultErrors.InvalidCredentials);
        }

        var response = new LoginResponseDto
        {
            AccessToken = string.Empty,
            RefreshToken = string.Empty
        };

        return Result.Success(response);
    }
}
