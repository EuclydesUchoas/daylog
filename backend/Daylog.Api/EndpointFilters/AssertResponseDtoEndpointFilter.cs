using Daylog.Application.Abstractions.Dtos;
using Daylog.Application.Common.Results;

namespace Daylog.Api.EndpointFilters;

public class AssertResponseDtoEndpointFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);

        if (result is INestedHttpResult resultData1)
        {
            result = resultData1.Result;
        }

        if (result is IValueHttpResult resultData2)
        {
            var resultValue = resultData2.Value;

            if (resultValue is null)
            {
                return result;
            }

            var resultValueType = resultValue.GetType();

            if (resultValueType.IsGenericType && resultValueType.GetGenericTypeDefinition() == typeof(Result<>))
            {
                var resultDataType = resultValueType.GetGenericArguments()[0];

                bool implementsIResponseDto = typeof(IResponseDto).IsAssignableFrom(resultDataType);

                if (!implementsIResponseDto)
                {
                    throw new InvalidOperationException($"The type {resultDataType.Name} must implement IResponseDto.");
                }
            }
        }

        return result;
    }
}
