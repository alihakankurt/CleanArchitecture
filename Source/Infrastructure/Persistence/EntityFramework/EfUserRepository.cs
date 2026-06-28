using Application.Persistence;
using Core.Infrastructure.Persistence.EntityFramework;
using Domain.Entities;

namespace Infrastructure.Persistence.EntityFramework;

public sealed class EfUserRepository : EfRepositoryBase<EfDatabaseContext, User, Guid>, IUserRepository
{
    public EfUserRepository(EfDatabaseContext context) : base(context)
    {
    }
}
