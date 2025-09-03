using Application.Features.Users.Exceptions;
using Application.Persistence;
using Core.Application.BusinessRules;

namespace Application.Features.Users;

internal sealed class UsersBusinessRules : IBusinessRules
{
    private readonly IUserRepository _userRepository;

    public UsersBusinessRules(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    internal async ValueTask EmailCanNotBeUsedTwiceOnRegistration(string email, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.AnyAsync(x => x.Email == email, cancellationToken))
            throw new EmailAlreadyUsedException();
    }
}
