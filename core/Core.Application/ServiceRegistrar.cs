using System.Reflection;
using Core.Application.BusinessRules;
using Core.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application;

internal static class ServiceRegistrar
{
    internal static void RegisterMediator(IServiceCollection services, Assembly assembly, ServiceLifetime serviceLifetime)
    {
        services.Add(ServiceDescriptor.Describe(typeof(IMediator), typeof(Mediator), serviceLifetime));
        services.Add(ServiceDescriptor.Describe(typeof(IRequestSender), (sp) => sp.GetRequiredService<IMediator>(), serviceLifetime));
        AddHandlers(services, assembly.DefinedTypes, serviceLifetime);
    }

    internal static void RegisterBusinessRules(IServiceCollection services, Assembly assembly, ServiceLifetime serviceLifetime)
    {
        AddBusinessRules(services, assembly.DefinedTypes, serviceLifetime);
    }

    private static void AddHandlers(IServiceCollection services, IEnumerable<TypeInfo> types, ServiceLifetime serviceLifetime)
    {
        var handlerTypes = new Type[] { typeof(IRequestHandler<>), typeof(IRequestHandler<,>) };

        foreach (var type in types)
        {
            if (type.IsGenericTypeDefinition || type.ContainsGenericParameters)
                continue;

            var interfaces = type.GetInterfaces().Where(static (iface) => iface.IsGenericType).ToArray();

            foreach (var handlerType in handlerTypes)
                if (interfaces.FirstOrDefault((iface) => iface.GetGenericTypeDefinition().IsAssignableTo(handlerType)) is Type serviceType)
                    services.Add(ServiceDescriptor.Describe(serviceType, type, serviceLifetime));
        }
    }

    private static void AddBusinessRules(IServiceCollection services, IEnumerable<TypeInfo> types, ServiceLifetime serviceLifetime)
    {
        var businessRulesType = typeof(IBusinessRules);
        foreach (var type in types)
            if (type.ImplementedInterfaces.Any((iface) => iface.IsAssignableTo(businessRulesType)))
                services.Add(ServiceDescriptor.Describe(type, type, serviceLifetime));
    }
}
