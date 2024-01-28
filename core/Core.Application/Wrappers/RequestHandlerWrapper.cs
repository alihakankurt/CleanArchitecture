using Core.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application.Wrappers;

internal interface IRequestHandlerWrapper
{
    public Task HandleAsync(IRequest request, IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}

internal interface IRequestHandlerWrapper<TResponse>
{
    public Task<TResponse> HandleAsync(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}

internal sealed class RequestHandlerWrapper<TRequest> : IRequestHandlerWrapper where TRequest : IRequest
{
    public async Task HandleAsync(IRequest request, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
        {
            serviceProvider.GetRequiredService<IRequestHandler<TRequest>>().HandleAsync(request, cancellationToken);
            return Unit.Task;
        }

        await serviceProvider.GetServices<IRequestPipeline<TRequest, Unit>>()
            .Reverse()
            .Aggregate((RequestHandlerDelegate<TRequest, Unit>)Handle,
                (next, pipeline) => (request, cancellationToken) => pipeline.HandleAsync(request, next, cancellationToken))((TRequest)request, cancellationToken);
    }
}

internal sealed class RequestHandlerWrapper<TRequest, TResponse> : IRequestHandlerWrapper<TResponse> where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> HandleAsync(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            return serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>().HandleAsync(request, cancellationToken);
        }

        return await serviceProvider.GetServices<IRequestPipeline<TRequest, TResponse>>()
            .Reverse()
            .Aggregate((RequestHandlerDelegate<TRequest, TResponse>)Handle,
                (next, pipeline) => (request, cancellationToken) => pipeline.HandleAsync(request, next, cancellationToken))((TRequest)request, cancellationToken);
    }
}
