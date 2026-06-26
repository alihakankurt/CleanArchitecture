using System.Collections.Concurrent;
using Core.Application.Requests;
using Core.Application.Wrappers;
using Core.Domain.Events;

namespace Core.Application;

internal sealed class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConcurrentDictionary<Type, object> _requestHandlers = new();
    private readonly ConcurrentDictionary<Type, object> _domainEventHandlers = new();

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ValueTask SendAsync(IRequest request, CancellationToken cancellationToken = default)
    {
        var handler = _requestHandlers.GetOrAdd(request.GetType(), static (requestType) =>
            Activator.CreateInstance(typeof(RequestHandlerWrapper<>).MakeGenericType(requestType))
                ?? throw new InvalidOperationException($"Could not create handler for request type {requestType}"));

        return ((IRequestHandlerWrapper)handler).HandleAsync(request, _serviceProvider, cancellationToken);
    }

    public ValueTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handler = _requestHandlers.GetOrAdd(request.GetType(), static (requestType) =>
            Activator.CreateInstance(typeof(RequestHandlerWrapper<,>).MakeGenericType(requestType, typeof(TResponse)))
                ?? throw new InvalidOperationException($"Could not create handler for request type {requestType}"));

        return ((IRequestHandlerWrapper<TResponse>)handler).HandleAsync(request, _serviceProvider, cancellationToken);
    }

    public ValueTask PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var handler = _domainEventHandlers.GetOrAdd(domainEvent.GetType(), static (domainEventType) =>
            Activator.CreateInstance(typeof(DomainEventHandlerWrapper<>).MakeGenericType(domainEventType))
                ?? throw new InvalidOperationException($"Could not create handler for domain event type {domainEventType}"));

        return ((IDomainEventHandlerWrapper)handler).HandleAsync(domainEvent, _serviceProvider, cancellationToken);
    }
}
