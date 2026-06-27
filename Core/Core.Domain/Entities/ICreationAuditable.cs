namespace Core.Domain.Entities;

/// <summary>
/// Marks an entity as having its creation date auditable.
/// </summary>
public interface ICreationAuditable
{
    /// <summary>
    /// Gets the date the entity was created at.
    /// </summary>
    public DateTimeOffset CreatedAt { get; }
}
