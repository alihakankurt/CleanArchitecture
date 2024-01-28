using System.Linq.Expressions;
using Core.Domain.Entities;

namespace Core.Application.Persistence.Repositories;

public interface IRepository<TEntity, TId> where TEntity : Entity<TId> where TId : notnull
{
    public void Create(TEntity entity);
    public void Update(TEntity entity);
    public void Delete(TEntity entity);
    public void DeleteById(TId id);
    public TEntity? Get(Expression<Func<TEntity, bool>> predicate);
    public TEntity? GetById(TId id);
    public List<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = default, Expression<Func<TEntity, object>>? orderBy = default);
    public bool Any(Expression<Func<TEntity, bool>> predicate);
    public IQueryable<TEntity> AsQueryable();
}
