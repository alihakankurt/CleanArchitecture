using System.Linq.Expressions;
using Core.Application.Persistence;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence.EntityFramework;

/// <summary>
/// Represents the base class implementation of <see cref="IRepository{TEntity, TId}"/> for EF Core.
/// </summary>
public abstract class EfRepositoryBase<TContext, TEntity, TId> : IRepository<TEntity, TId>
    where TContext : DbContext
    where TEntity : Entity<TId>
    where TId : notnull
{
    protected TContext Context { get; }

    protected EfRepositoryBase(TContext context)
    {
        Context = context;
    }

    public async ValueTask<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().FindAsync([id], cancellationToken);
    }

    public async ValueTask<TEntity?> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async ValueTask<List<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.ToListAsync(cancellationToken);
    }

    public async ValueTask<long> CountAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.LongCountAsync(cancellationToken);
    }

    public async ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (predicate is not null)
            query = query.Where(predicate);

        return await query.AnyAsync(cancellationToken);
    }

    public async ValueTask CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Context.AddAsync(entity, cancellationToken);
    }

    public ValueTask UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Context.Update(entity);
        return ValueTask.CompletedTask;
    }

    public ValueTask DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Context.Remove(entity);
        return ValueTask.CompletedTask;
    }

    public async ValueTask DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        await Context.Set<TEntity>().Where((entity) => entity.Id.Equals(id)).ExecuteDeleteAsync(cancellationToken);
    }
}
