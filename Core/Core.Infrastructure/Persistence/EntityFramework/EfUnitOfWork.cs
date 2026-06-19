using Core.Application.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence.EntityFramework;

/// <summary>
/// Represents the implementation of <see cref="IUnitOfWork"/> for EF Core.
/// </summary>
public sealed class EfUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
    private readonly TContext _context;

    public EfUnitOfWork(TContext context)
    {
        _context = context;
    }

    public async ValueTask<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return new EfTransaction(await _context.Database.BeginTransactionAsync(cancellationToken));
    }

    public async ValueTask SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
