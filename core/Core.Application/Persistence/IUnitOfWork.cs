namespace Core.Application.Persistence;

/// <summary>
/// Represents a transactional boundary that coordinates and commits changes across multiple repositories as a single atomic operation.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Begins a new transaction operation.
    /// </summary>
    /// <returns>A new instance of <see cref="ITransaction"/>.</returns>
    public ValueTask<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves the changes to the database.
    /// </summary>
    public ValueTask SaveChangesAsync(CancellationToken cancellationToken = default);
}
