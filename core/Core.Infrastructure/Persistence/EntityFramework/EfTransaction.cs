using Core.Application.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core.Infrastructure.Persistence.EntityFramework;

/// <summary>
/// Represents the implementation of <see cref="ITransaction"/> for EF Core.
/// </summary>
public sealed class EfTransaction : ITransaction
{
    private readonly IDbContextTransaction _instance;

    internal EfTransaction(IDbContextTransaction instance)
    {
        _instance = instance;
    }

    public async ValueTask CommitAsync(CancellationToken cancellationToken = default)
    {
        await _instance.CommitAsync(cancellationToken);
    }

    public async ValueTask RollbackAsync(CancellationToken cancellationToken = default)
    {
        await _instance.RollbackAsync(cancellationToken);
    }
}
