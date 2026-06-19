using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Persistence;

/// <summary>
/// Represents an abstraction for accessing and managing domain entities in a persistent storage.
/// </summary>
public interface IRepository<TEntity, TId> where TEntity : Entity<TId> where TId : notnull
{
    /// <summary>
    /// Tries to find an entity with the <paramref name="id"/>.
    /// </summary>
    public ValueTask<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Tries to find first entity that matches the <paramref name="predicate"/>.
    /// </summary>
    public ValueTask<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds all entities that matches the <paramref name="predicate"/> if provided; otherwise, returns all entities.
    /// </summary>
    public ValueTask<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts the number of entities that matches the <paramref name="predicate"/> if provided; otherwise, counts all entities.
    /// </summary>
    /// <returns>The number of entities.</returns>
    public ValueTask<long> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks whether an entity exists that matches the <paramref name="predicate"/> if provided; otherwise, checks if any entity exists.
    /// </summary>
    /// <returns>Whether there is an entity or not.</returns>
    public ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new entity in storage.
    /// </summary>
    public ValueTask CreateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the entity by matching the primary key.
    /// </summary>
    public ValueTask UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the entity by matching the primary key.
    /// </summary>
    public ValueTask DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the entity by matching the primary key.
    /// </summary>
    public ValueTask DeleteByIdAsync(TId id, CancellationToken cancellationToken = default);
}
