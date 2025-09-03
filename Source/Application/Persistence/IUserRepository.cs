using Core.Application.Persistence;
using Domain.Entities;

namespace Application.Persistence;

public interface IUserRepository : IRepository<User, long>
{
}
