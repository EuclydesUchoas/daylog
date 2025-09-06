using Daylog.Application.Abstractions.Services;
using Daylog.Application.Shared.Results;
using Daylog.Application.Users.Dtos.Request;

namespace Daylog.Application.Users.Services.Contracts;

public interface IDeleteUserService : IService<DeleteUserRequestDto, Result>;
