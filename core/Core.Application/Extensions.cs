using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

public static class Extensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        ServiceRegistrar.RegisterMediator(services, Assembly.GetCallingAssembly(), ServiceLifetime.Scoped);
        return services;
    }

    public static IServiceCollection AddBusinessRules(this IServiceCollection services)
    {
        ServiceRegistrar.RegisterBusinessRules(services, Assembly.GetCallingAssembly(), ServiceLifetime.Scoped);
        return services;
    }
}
