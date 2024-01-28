namespace Core.Application.Persistence;

public interface IUnitOfWork
{
    public void CreateTransaction();
    public ValueTask CreateTransactionAsync(CancellationToken cancellationToken = default);
    public void SaveChanges();
    public ValueTask SaveChangesAsync(CancellationToken cancellationToken = default);
    public void Commit();
    public ValueTask CommitAsync(CancellationToken cancellationToken = default);
    public void Rollback();
    public ValueTask RollbackAsync(CancellationToken cancellationToken = default);
}
