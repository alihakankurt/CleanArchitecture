using Application.Persistence.Repositories;
using Core.Infrastructure.Persistence.EntityFramework;
using Domain.Entities;

namespace Infrastructure.Persistence.EntityFramework.Repositories;

public sealed class EfUserRepository : EfRepositoryBase<EfDatabaseContext, User, int>, IUserRepository
{
    public EfUserRepository(EfDatabaseContext context) : base(context)
    {
    }
}
