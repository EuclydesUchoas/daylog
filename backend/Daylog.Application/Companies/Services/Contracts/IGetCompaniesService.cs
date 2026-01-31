using Daylog.Application.Abstractions.Services;
using Daylog.Application.Common.Dtos.Response;
using Daylog.Application.Companies.Dtos.Request;
using Daylog.Application.Companies.Dtos.Response;

namespace Daylog.Application.Companies.Services.Contracts;

public interface IGetCompaniesService : IService<GetCompaniesRequestDto, ICollectionResponseDto<CompanyResponseDto>>;
