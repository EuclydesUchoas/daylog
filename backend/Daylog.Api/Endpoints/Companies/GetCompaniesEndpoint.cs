using Daylog.Application.Common.Dtos.Response;
using Daylog.Shared.Core.Resources;
using Daylog.Application.Common.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Daylog.Application.Companies.Dtos.Response;
using Daylog.Application.Companies.Dtos.Request;
using Daylog.Application.Companies.Services.Contracts;

namespace Daylog.Api.Endpoints.Companies;

public sealed class GetCompaniesEndpoint : IEndpoint
{
    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapGet("v1/companies", HandleAsync)
            .WithSummary(nameof(AppMessages.Endpoint_GetCompaniesSummary))
            .WithDescription(nameof(AppMessages.Endpoint_GetCompaniesDescription))
            .RequireAuthorization()
            .WithTags(EndpointTags.Companies);
    }

    public static async Task<Results<Ok<Result<ICollectionResponseDto<CompanyResponseDto>>>, BadRequest<Result>, NotFound<Result<ICollectionResponseDto<CompanyResponseDto>>>>> HandleAsync(
        [AsParameters] GetCompaniesRequestDto getCompaniesRequestDto,
        [FromServices] IGetCompaniesService getCompaniesService,
        CancellationToken cancellationToken
        )
    {
        var result = await getCompaniesService.HandleAsync(getCompaniesRequestDto, cancellationToken);

        if (result.IsSuccess)
        {
            return (result.Data?.Items?.Any() ?? false)
                ? TypedResults.Ok(result)
                : TypedResults.NotFound(result);
        }

        return TypedResults.BadRequest(result.Base);
    }
}
