using Core.Application.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Infrastructure.Persistence;

public sealed class EfUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _context;
    private IDbContextTransaction? _transaction;

    public EfUnitOfWork(TContext context)
    {
        _context = context;
    }

    public void CreateTransaction()
    {
        if (_transaction is not null)
            throw new InvalidOperationException("A transaction is already created.");

        _transaction = _context.Database.BeginTransaction();
    }

    public async ValueTask CreateTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
            throw new InvalidOperationException("A transaction is already created.");

        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }

    public async ValueTask SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Commit()
    {
        _transaction?.Commit();
        _transaction?.Dispose();
        _transaction = null;
    }

    public async ValueTask CommitAsync(CancellationToken cancellationToken = default)
    {
        await (_transaction?.CommitAsync(cancellationToken) ?? Task.CompletedTask);
        await (_transaction?.DisposeAsync() ?? ValueTask.CompletedTask);
        _transaction = null;
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transaction = null;
    }

    public async ValueTask RollbackAsync(CancellationToken cancellationToken = default)
    {
        await (_transaction?.RollbackAsync(cancellationToken) ?? Task.CompletedTask);
        await (_transaction?.DisposeAsync() ?? ValueTask.CompletedTask);
        _transaction = null;
    }
}
