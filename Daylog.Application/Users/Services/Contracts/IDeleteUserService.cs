using Daylog.Application.Abstractions.Services;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;

namespace Daylog.Application.Users.Services.Contracts;

public interface IDeleteUserService : IService<DeleteUserRequestDto, Result>;
