using Core.Application.Requests;
using Core.Application.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application.Wrappers;

internal interface IRequestHandlerWrapper
{
    public ValueTask HandleAsync(IRequest request, IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}

internal sealed class RequestHandlerWrapper<TRequest> : IRequestHandlerWrapper where TRequest : IRequest
{
    public async ValueTask HandleAsync(IRequest request, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest>>();
        await serviceProvider.GetServices<IRequestPipeline<TRequest>>()
            .Reverse()
            .Aggregate((NextHandlerDelegate<TRequest>)(() => handler.HandleAsync((TRequest)request, cancellationToken)),
                (next, pipeline) => (NextHandlerDelegate<TRequest>)(() => pipeline.HandleAsync((TRequest)request, next, cancellationToken)))();
    }
}

internal interface IRequestHandlerWrapper<TResponse>
{
    public ValueTask<TResponse> HandleAsync(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}

internal sealed class RequestHandlerWrapper<TRequest, TResponse> : IRequestHandlerWrapper<TResponse> where TRequest : IRequest<TResponse>
{
    public async ValueTask<TResponse> HandleAsync(IRequest<TResponse> request, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var handler = serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
        return await serviceProvider.GetServices<IRequestPipeline<TRequest, TResponse>>()
            .Reverse()
            .Aggregate((NextHandlerDelegate<TRequest, TResponse>)(() => handler.HandleAsync((TRequest)request, cancellationToken)),
                (next, pipeline) => (NextHandlerDelegate<TRequest, TResponse>)(() => pipeline.HandleAsync((TRequest)request, next, cancellationToken)))();
    }
}
