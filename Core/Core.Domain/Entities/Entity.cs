using Core.Domain.Events;

namespace Core.Domain.Entities;

/// <summary>
/// Marks a type as an entity. Use <see cref="Entity"/> or <see cref="Entity{TId}"/> instead of this.
/// </summary>
public interface IEntity
{
}

/// <summary>
/// Represents the base type of entities without a primary key.
/// </summary>
public abstract class Entity : IEntity
{
}

/// <summary>
/// Represents the base type of entities with a primary key.
/// </summary>
/// <typeparam name="TId">The type of the primary key.</typeparam>
public abstract class Entity<TId> : IEntity where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets the domain events.
    /// </summary>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Gets the primary key.
    /// </summary>
    public TId Id { get; protected set; } = default!;

    /// <summary>
    /// Raises a new domain event.
    /// </summary>
    /// <param name="domainEvent">The event to raise.</param>
    protected void RaiseEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Removes and returns the domain events.
    /// </summary>
    /// <returns>A <see cref="List{IDomainEvent}"/> that contains the events.</returns>
    public List<IDomainEvent> ReleaseDomainEvents()
    {
        var domainEvents = _domainEvents.ToList();
        _domainEvents.Clear();
        return domainEvents;
    }
}
