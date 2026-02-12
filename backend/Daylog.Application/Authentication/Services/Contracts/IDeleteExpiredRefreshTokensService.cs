using Daylog.Application.Abstractions.Services;
using Daylog.Application.Authentication.Dtos.Request;
using Daylog.Application.Authentication.Dtos.Response;

namespace Daylog.Application.Authentication.Services.Contracts;

public interface IDeleteExpiredRefreshTokensService : IService<DeleteExpiredRefreshTokensRequestDto, DeleteExpiredRefreshTokensResponseDto>;
