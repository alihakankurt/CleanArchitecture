namespace Core.Domain.Entities;

/// <summary>
/// Marks an entity as having its update date auditable.
/// </summary>
public interface IUpdateAuditable
{
    /// <summary>
    /// Gets the date the entity was updated at.
    /// </summary>
    public DateTimeOffset UpdatedAt { get; }
}
