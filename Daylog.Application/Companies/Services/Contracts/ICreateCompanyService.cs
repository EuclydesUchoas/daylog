using Daylog.Application.Abstractions.Services;
using Daylog.Application.Companies.Dtos.Request;
using Daylog.Application.Companies.Dtos.Response;

namespace Daylog.Application.Companies.Services.Contracts;

public interface ICreateCompanyService : IService<CreateCompanyRequestDto, CompanyResponseDto>;
