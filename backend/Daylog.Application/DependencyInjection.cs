using Daylog.Application.Abstractions.Authentication;
using Daylog.Application.Abstractions.Services;
using Daylog.Application.Authentication;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Daylog.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblies(ApplicationAssemblyReference.Assembly)
            .AddClasses(classes => classes.AssignableToAny(typeof(IService<,>), typeof(IService<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddValidatorsFromAssembly(ApplicationAssemblyReference.Assembly, includeInternalTypes: true);

        return services;
    }
}
