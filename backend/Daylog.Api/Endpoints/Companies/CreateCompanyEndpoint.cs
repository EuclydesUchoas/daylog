using Daylog.Shared.Core.Resources;
using Daylog.Application.Common.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Daylog.Application.Companies.Dtos.Request;
using Daylog.Application.Companies.Dtos.Response;
using Daylog.Application.Companies.Services.Contracts;

namespace Daylog.Api.Endpoints.Companies;

public sealed class CreateCompanyEndpoint : IEndpoint
{
    public void MapRoute(IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder
            .MapPost("v1/companies", HandleAsync)
            .WithSummary(nameof(AppMessages.Endpoint_CreateCompanySummary))
            .WithDescription(nameof(AppMessages.Endpoint_CreateCompanyDescription))
            .RequireAuthorization()
            .WithTags(EndpointTags.Companies);
    }

    public static async Task<Results<Ok<Result<CompanyResponseDto>>, BadRequest<Result>>> HandleAsync(
        [FromBody] CreateCompanyRequestDto createCompanyRequestDto,
        [FromServices] ICreateCompanyService createCompanyService,
        CancellationToken cancellationToken
        )
    {
        var result = await createCompanyService.HandleAsync(createCompanyRequestDto, cancellationToken);

        if (result.IsSuccess)
        {
            return TypedResults.Ok(result);
        }

        return result.Error?.Type switch
        {
            _ => TypedResults.BadRequest(result.Base)
        };
    }
}
