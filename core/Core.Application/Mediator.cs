using System.Collections.Concurrent;
using Core.Application.Contracts;
using Core.Application.Wrappers;

namespace Core.Application;

public class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, object> _requestHandlers = new();

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        var handler = _requestHandlers.GetOrAdd(request.GetType(), static (requestType) =>
            Activator.CreateInstance(typeof(RequestHandlerWrapper<>).MakeGenericType(requestType))
                ?? throw new InvalidOperationException($"Could not create handler for request type {requestType}"));

        return ((IRequestHandlerWrapper)handler).HandleAsync(request, _serviceProvider, cancellationToken);
    }

    public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handler = _requestHandlers.GetOrAdd(request.GetType(), static (requestType) =>
            Activator.CreateInstance(typeof(RequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse)))
                ?? throw new InvalidOperationException($"Could not create handler for request type {requestType}"));

        return ((IRequestHandlerWrapper<TResponse>)handler).HandleAsync(request, _serviceProvider, cancellationToken);
    }
}
