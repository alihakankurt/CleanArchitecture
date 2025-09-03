using System.Reflection;
using Core.Application.BusinessRules;
using Core.Application.Requests;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

internal static class ServiceRegistrar
{
    internal static void RegisterMediatorServices(IServiceCollection services, ServiceLifetime lifetime, Assembly assembly)
    {
        AddMediator(services, lifetime);
        AddRequestHandlers(services, lifetime, assembly.DefinedTypes);
    }

    internal static void RegisterBusinessRules(IServiceCollection services, ServiceLifetime serviceLifetime, Assembly assembly)
    {
        AddBusinessRules(services, serviceLifetime, assembly.DefinedTypes);
    }

    private static void AddMediator(IServiceCollection services, ServiceLifetime lifetime)
    {
        services.Add(ServiceDescriptor.Describe(typeof(IMediator), typeof(Mediator), lifetime));
    }

    private static void AddRequestHandlers(IServiceCollection services, ServiceLifetime lifetime, IEnumerable<TypeInfo> types)
    {
        Type[] handlerTypes = [typeof(IRequestHandler<>), typeof(IRequestHandler<,>)];

        foreach (var type in types)
        {
            if (type.IsGenericTypeDefinition || type.ContainsGenericParameters)
                continue;

            var interfaces = type.GetInterfaces().Where(static (iface) => iface.IsGenericType).ToArray();

            if (interfaces.FirstOrDefault((iface) => handlerTypes.Contains(iface.GetGenericTypeDefinition())) is Type serviceType)
                services.Add(ServiceDescriptor.Describe(serviceType, type, lifetime));
        }
    }

    private static void AddBusinessRules(IServiceCollection services, ServiceLifetime lifetime, IEnumerable<TypeInfo> types)
    {
        foreach (var type in types.Where((type) => type.ImplementedInterfaces.Any((iface) => iface.IsAssignableTo(typeof(IBusinessRules)))))
            services.Add(ServiceDescriptor.Describe(type, type, lifetime));
    }
}
