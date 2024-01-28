using System.Linq.Expressions;
using Core.Application.Persistence.Repositories;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Persistence.EntityFramework;

public abstract class EfRepositoryBase<TContext, TEntity, TId> : IRepository<TEntity, TId>, IAsyncRepository<TEntity, TId>
    where TContext : DbContext
    where TEntity : Entity<TId>
    where TId : notnull
{
    protected TContext Context { get; }

    protected EfRepositoryBase(TContext context)
    {
        Context = context;
    }

    public void Create(TEntity entity)
    {
        Context.Add(entity);
    }

    public ValueTask CreateAsync(TEntity entity)
    {
        Context.Add(entity);
        return ValueTask.CompletedTask;
    }

    public void Update(TEntity entity)
    {
        Context.Update(entity);
    }

    public ValueTask UpdateAsync(TEntity entity)
    {
        Context.Update(entity);
        return ValueTask.CompletedTask;
    }

    public void Delete(TEntity entity)
    {
        Context.Remove(entity);
    }

    public ValueTask DeleteAsync(TEntity entity)
    {
        Context.Remove(entity);
        return ValueTask.CompletedTask;
    }

    public void DeleteById(TId id)
    {
        if (GetById(id) is TEntity entity)
            Context.Remove(entity);
    }

    public async ValueTask DeleteByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        if (await GetByIdAsync(id, cancellationToken) is TEntity entity)
            Context.Remove(entity);
    }

    public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
    {
        return Context.Set<TEntity>().FirstOrDefault(predicate);
    }

    public async ValueTask<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public TEntity? GetById(TId id)
    {
        return Context.Set<TEntity>().FirstOrDefault((entity) => entity.Id.Equals(id));
    }

    public async ValueTask<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().FirstOrDefaultAsync((entity) => entity.Id.Equals(id), cancellationToken);
    }

    public List<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, object>>? orderBy = default)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (predicate is not null)
            query = query.Where(predicate);

        if (orderBy is not null)
            query.OrderBy(orderBy);

        return query.ToList();
    }

    public async ValueTask<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, Expression<Func<TEntity, object>>? orderBy = default, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = Context.Set<TEntity>();

        if (predicate is not null)
            query = query.Where(predicate);

        if (orderBy is not null)
            query.OrderBy(orderBy);

        return await query.ToListAsync(cancellationToken);
    }

    public bool Any(Expression<Func<TEntity, bool>> predicate)
    {
        return Context.Set<TEntity>().Any(predicate);
    }

    public async ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await Context.Set<TEntity>().AnyAsync(predicate, cancellationToken);
    }

    public IQueryable<TEntity> AsQueryable()
    {
        return Context.Set<TEntity>();
    }

    public IQueryable<TEntity> AsQueryableAsync()
    {
        return Context.Set<TEntity>();
    }
}
