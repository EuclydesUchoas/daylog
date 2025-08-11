using Daylog.Application.Helpers.Configuration;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Daylog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IConfigurationHelper, ConfigurationHelper>();

        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(ApplicationAssemblyReference.Assembly);
        });

        services.AddValidatorsFromAssembly(ApplicationAssemblyReference.Assembly, includeInternalTypes: true);

        return services;
    }
}
