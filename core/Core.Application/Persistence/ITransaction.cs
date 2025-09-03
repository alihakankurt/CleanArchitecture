namespace Core.Application.Persistence;

/// <summary>
/// Represents a sequence of operations performed as a single, indivisible unit, ensuring that all operations succeed or fail together to maintain data integrity.
/// </summary>
public interface ITransaction
{
    /// <summary>
    /// Commits the operations to the database.
    /// </summary>
    public ValueTask CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rollbacks and drops the operations.
    /// </summary>
    public ValueTask RollbackAsync(CancellationToken cancellationToken = default);
}
