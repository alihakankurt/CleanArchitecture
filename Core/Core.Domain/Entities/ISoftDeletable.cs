namespace Core.Domain.Entities;

/// <summary>
/// Marks the entity as soft deletable.
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Gets the date the entity was soft-deleted at.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Gets whether the entity is soft-deleted or not.
    /// </summary>
    public bool IsDeleted => DeletedAt is not null;
}
