using Core.Domain.Events;

namespace Core.Application.Events;

/// <summary>
/// Represents the base type of domain event handlers.
/// </summary>
public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
{
    public ValueTask HandleAsync(TDomainEvent domainEvent, CancellationToken cancellationToken = default);
}
