using Microsoft.Extensions.DependencyInjection;
using Core.Application;
using Core.Application.Contracts;
using Application.Pipelines;

namespace Application;

public static class ApplicationServiceRegistrar
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediator();
        services.AddBusinessRules();

        services.AddScoped(typeof(IRequestPipeline<,>), typeof(LoggingPipeline<,>));

        return services;
    }
}
