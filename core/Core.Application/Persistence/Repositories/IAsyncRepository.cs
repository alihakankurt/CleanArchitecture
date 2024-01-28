using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Persistence.Repositories;

public interface IAsyncRepository<TEntity, TId> where TEntity : Entity<TId> where TId : notnull
{
    public ValueTask CreateAsync(TEntity entity);
    public ValueTask UpdateAsync(TEntity entity);
    public ValueTask DeleteAsync(TEntity entity);
    public ValueTask DeleteByIdAsync(TId id, CancellationToken cancellationToken = default);
    public ValueTask<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    public ValueTask<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    public ValueTask<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = default, Expression<Func<TEntity, object>>? orderBy = default, CancellationToken cancellationToken = default);
    public ValueTask<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    public IQueryable<TEntity> AsQueryableAsync();
}
