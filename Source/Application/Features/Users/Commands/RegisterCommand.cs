using Application.Features.Users.Dtos;
using Application.Persistence;
using Core.Application.Persistence;
using Core.Application.Requests;
using Core.Application.Services;
using Domain.Entities;

namespace Application.Features.Users.Commands;

public sealed record RegisterCommand(UserRegistrationDto UserRegistrationDto) : IRequest<RegisterResponse>;

public sealed record RegisterResponse(TokenInfo AccessToken, TokenInfo RefreshToken);

internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;
    private readonly ITokenService _tokenService;
    private readonly UsersBusinessRules _usersBusinessRules;

    public RegisterCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IHashService hashService,
        ITokenService tokenService, UsersBusinessRules usersBusinessRules)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _hashService = hashService;
        _tokenService = tokenService;
        _usersBusinessRules = usersBusinessRules;
    }

    public async ValueTask<RegisterResponse> HandleAsync(RegisterCommand command, CancellationToken cancellationToken = default)
    {
        await _usersBusinessRules.EmailCanNotBeUsedTwiceOnRegistration(command.UserRegistrationDto.Email, cancellationToken);

        _hashService.GeneratePasswordHash(command.UserRegistrationDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var user = new User(
            command.UserRegistrationDto.FirstName,
            command.UserRegistrationDto.LastName,
            command.UserRegistrationDto.Email,
            passwordHash, passwordSalt);

        await _userRepository.CreateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        TokenInfo refreshTokenInfo = _tokenService.GenerateRefreshToken();

        user.AddRefreshToken(
            refreshTokenInfo.Token,
            refreshTokenInfo.IssuedAt,
            refreshTokenInfo.ExpiresAt);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        TokenInfo accessTokenInfo = _tokenService.GenerateAccessToken(user.Id.ToString(), user.FullName, user.Email);
        return new RegisterResponse(accessTokenInfo, refreshTokenInfo);
    }
}
