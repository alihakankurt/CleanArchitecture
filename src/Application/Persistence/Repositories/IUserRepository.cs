using Core.Application.Persistence.Repositories;
using Domain.Entities;

namespace Application.Persistence.Repositories;

public interface IUserRepository : IRepository<User, int>, IAsyncRepository<User, int>
{
}
