using Daylog.Application.Helpers.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Daylog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IConfigurationHelper, ConfigurationHelper>();

        return services;
    }
}
