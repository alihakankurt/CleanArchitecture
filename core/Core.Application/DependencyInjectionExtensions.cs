using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds the mediator services to the <see cref="IServiceCollection"/> instance.
    /// </summary>
    public static IServiceCollection AddMediatorServices(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        ServiceRegistrar.RegisterMediatorServices(services, lifetime, Assembly.GetCallingAssembly());
        return services;
    }

    /// <summary>
    /// Adds the business rules implementations to the <see cref="IServiceCollection"/> instance.
    /// </summary>
    public static IServiceCollection AddBusinessRules(this IServiceCollection services, ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        ServiceRegistrar.RegisterBusinessRules(services, lifetime, Assembly.GetCallingAssembly());
        return services;
    }
}
