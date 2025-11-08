using Daylog.Application.Abstractions.Services;
using Daylog.Application.Common.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Domain.Users;

namespace Daylog.Application.Users.Services.Contracts;

public interface IUpdateUserService : IService<UpdateUserRequestDto, Result<User>>;
