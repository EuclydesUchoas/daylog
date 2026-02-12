using Daylog.Application.Abstractions.Dtos;
using System.Diagnostics.CodeAnalysis;

namespace Daylog.Application.Authentication.Dtos.Response;

public sealed class DeleteExpiredRefreshTokensResponseDto : IResponseDto
{
    public required int DeletedTokensCount { get; init; }

    public DeleteExpiredRefreshTokensResponseDto() { }

    [SetsRequiredMembers]
    public DeleteExpiredRefreshTokensResponseDto(int deletedTokensCount)
    {
        DeletedTokensCount = deletedTokensCount;
    }
}
