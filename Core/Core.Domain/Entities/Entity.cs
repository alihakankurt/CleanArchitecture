using Core.Domain.Events;

namespace Core.Domain.Entities;

/// <summary>
/// Marks a type as an entity. Use <see cref="Entity"/> or <see cref="Entity{TId}"/> instead of this.
/// </summary>
public interface IEntity
{
}

/// <summary>
/// Represents the base type of weak entities.
/// </summary>
public abstract class Entity : IEntity
{
}

/// <summary>
/// Represents the base type of entities with primary key.
/// </summary>
/// <typeparam name="TId">The type of the primary key.</typeparam>
public abstract class Entity<TId> : IEntity where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents;

    /// <summary>
    /// Gets the domain events.
    /// </summary>
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents;

    /// <summary>
    /// Gets the primary key.
    /// </summary>
    public TId Id { get; protected set; } = default!;

    /// <summary>
    /// Initializes a new instance of <see cref="Entity"/>.
    /// </summary>
    protected Entity()
    {
        _domainEvents = new List<IDomainEvent>();
    }

    /// <summary>
    /// Raises a new domain event.
    /// </summary>
    /// <param name="domainEvent">The event to raise.</param>
    public void RaiseEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears the domain events.
    /// </summary>
    public void ClearEvents()
    {
        _domainEvents.Clear();
    }
}
