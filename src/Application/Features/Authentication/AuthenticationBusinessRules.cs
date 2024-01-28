using Application.Features.Authentication.Exceptions;
using Application.Persistence.Repositories;
using Core.Application.BusinessRules;
using Core.Application.Services;
using Domain.Entities;

namespace Application.Features.Authentication;

internal sealed class AuthenticationBusinessRules : IBusinessRules
{
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;

    public AuthenticationBusinessRules(IUserRepository userRepository, IHashService hashService)
    {
        _userRepository = userRepository;
        _hashService = hashService;
    }

    internal async ValueTask EmailCanNotBeUsedTwiceOnRegistration(string email, CancellationToken cancellationToken = default)
    {
        if (await _userRepository.AnyAsync(x => x.Email == email, cancellationToken))
            throw new EmailAlreadyUsedException();
    }

    internal ValueTask PasswordMustBeConfirmedOnRegistration(string password, string confirmedPassword)
    {
        if (password != confirmedPassword)
            throw new PasswordNotConfirmedException();

        return ValueTask.CompletedTask;
    }

    internal ValueTask UserMustBeExistToUseLogIn(User? user)
    {
        _ = user ?? throw new InvalidEmailOrPasswordException();
        return ValueTask.CompletedTask;
    }

    internal ValueTask PasswordMustBeValidOnLogin(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        if (!_hashService.ValidatePasswordHash(password, passwordHash, passwordSalt))
            throw new InvalidEmailOrPasswordException();

        return ValueTask.CompletedTask;
    }

    internal ValueTask UserMustBeExistToUseRefreshToken(User? user)
    {
        _ = user ?? throw new InvalidRefreshTokenException();
        return ValueTask.CompletedTask;
    }

    internal ValueTask RefreshTokenMustBeExistToUse(RefreshToken? refreshToken)
    {
        _ = refreshToken ?? throw new InvalidRefreshTokenException();
        return ValueTask.CompletedTask;
    }
}
