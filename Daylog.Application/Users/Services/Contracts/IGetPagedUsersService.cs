using Daylog.Application.Abstractions.Services;
using Daylog.Application.Shared;
using Daylog.Application.Shared.Results;
using Daylog.Application.Users.Dtos.Request;
using Daylog.Domain.Users;

namespace Daylog.Application.Users.Services.Contracts;

//public interface IGetPagedUsersService : IService<GetPagedUsersRequestDto, Result<PagedEntity<User>>>;
public interface IGetPagedUsersService : IService<GetPagedUsersRequestDto, Result<PagedData<User>>>;
