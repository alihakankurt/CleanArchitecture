using Core.Application.Events;
using Core.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Application.Wrappers;

internal interface IDomainEventHandlerWrapper
{
    public ValueTask HandleAsync(IDomainEvent domainEvent, IServiceProvider serviceProvider, CancellationToken cancellationToken = default);
}

internal sealed class DomainEventHandlerWrapper<TDomainEvent> : IDomainEventHandlerWrapper where TDomainEvent : IDomainEvent
{
    public async ValueTask HandleAsync(IDomainEvent domainEvent, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var handlers = serviceProvider.GetServices<IDomainEventHandler<TDomainEvent>>();

        foreach (var handler in handlers)
        {
            await handler.HandleAsync((TDomainEvent)domainEvent, cancellationToken);
        }
    }
}
