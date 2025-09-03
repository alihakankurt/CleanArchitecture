using Microsoft.Extensions.DependencyInjection;
using Core.Application;
using Core.Application.Pipelines;
using Application.Pipelines;

namespace Application;

public static class ApplicationServiceRegistrar
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatorServices();
        services.AddBusinessRules();

        services.AddScoped(typeof(IRequestPipeline<>), typeof(LoggingPipeline<>));
        services.AddScoped(typeof(IRequestPipeline<,>), typeof(LoggingPipeline<,>));

        return services;
    }
}
