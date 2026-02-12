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
    ITokenService tokenService,
    ICreateRefreshTokenService createRefreshTokenService
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
                        Id = x.Profile.Id
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
            userAuth.UserInfo.Profile
            );

        var accessToken = tokenService.GenerateToken(userAuthInfo);
        var refreshToken = tokenService.GenerateRefreshToken();

        var createRefreshToken = new CreateRefreshTokenRequestDto(
            userAuth.UserInfo.Id,
            refreshToken.Token,
            refreshToken.ExpiresAt
            );

        var createRefreshTokenResult = await createRefreshTokenService.HandleAsync(createRefreshToken, cancellationToken);

        if (createRefreshTokenResult.IsFailure)
        {
            return Result.Failure<LoginResponseDto>(createRefreshTokenResult.Error);
        }

        var tokens = new TokensResponseDto
        {
            AccessToken = new TokenInfoResponseDto(accessToken.Token, accessToken.ExpiresAt),
            RefreshToken = new TokenInfoResponseDto(refreshToken.Token, refreshToken.ExpiresAt)
        };

        var response = new LoginResponseDto
        {
            UserInfo = userAuth.UserInfo,
            Tokens = tokens
        };

        return Result.Success(response);
    }
}
