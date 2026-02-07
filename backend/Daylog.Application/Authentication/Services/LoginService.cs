using Daylog.Application.Abstractions.Authentication;
using Daylog.Application.Abstractions.Data;
using Daylog.Application.Authentication.Dtos.Request;
using Daylog.Application.Authentication.Dtos.Response;
using Daylog.Application.Authentication.Models;
using Daylog.Application.Authentication.Results;
using Daylog.Application.Authentication.Services.Contracts;
using Daylog.Application.Common.Results;
using Daylog.Application.UserProfiles.Dtos.Response;
using Daylog.Shared.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Daylog.Application.Authentication.Services;

public sealed class LoginService(
    IAppDbContext appDbContext,
    IPasswordHasher passwordHasher,
    ITokenService tokenService
    ) : ILoginService
{
    public async Task<Result<LoginResponseDto>> HandleAsync(LoginRequestDto requestDto, CancellationToken cancellationToken = default)
    {
        if (requestDto is null)
        {
            return Result.Failure<LoginResponseDto>(ResultError.NullData);
        }

        var userAuth = await appDbContext.Users.AsNoTracking()
            .Search(x => x.Email, requestDto.Email)
            .Select(x => new
            {
                UserInfo = new LoginUserInfoResponseDto
                {
                    Id = x.Id,
                    Email = x.Email,
                    Name = x.Name,
                    Profile = new UserProfileResponseDto
                    {
                        Id = x.Profile.Id,
                    }
                },
                x.Password,
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (userAuth is null || !passwordHasher.Verify(requestDto.Password, userAuth.Password))
        {
            return Result.Failure<LoginResponseDto>(AuthResultErrors.InvalidCredentials);
        }

        var userAuthInfo = new UserAuthInfo(
            userAuth.UserInfo.Id,
            userAuth.UserInfo.Email,
            userAuth.UserInfo.Name,
            userAuth.UserInfo.Profile.Id
            );

        var accessToken = tokenService.GenerateToken(userAuthInfo);
        var refreshToken = tokenService.GenerateRefreshToken();

        // Temporary implementation
        var response = new LoginResponseDto
        {
            AccessToken = new LoginTokenInfoResponseDto(accessToken.Token, accessToken.ExpiresAt),
            RefreshToken = new LoginTokenInfoResponseDto(refreshToken.Token, refreshToken.ExpiresAt)
        };

        return Result.Success(response);
    }
}
