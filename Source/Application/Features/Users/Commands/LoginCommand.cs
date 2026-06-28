using Application.Features.Users.Dtos;
using Application.Features.Users.Exceptions;
using Application.Persistence;
using Core.Application.Persistence;
using Core.Application.Requests;
using Core.Application.Services;
using Domain.Entities;

namespace Application.Features.Users.Commands;

public readonly record struct LoginCommand(UserLoginDto UserLoginDto) : IRequest<LoginResponse>;

public readonly record struct LoginResponse(TokenInfo AccessToken, TokenInfo RefreshToken);

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;
    private readonly ITokenService _tokenService;
    private readonly IDateTimeService _dateTimeService;

    public LoginCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IHashService hashService, ITokenService tokenService, IDateTimeService dateTimeService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _hashService = hashService;
        _tokenService = tokenService;
        _dateTimeService = dateTimeService;
    }

    public async ValueTask<LoginResponse> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindAsync((user) => user.Email == command.UserLoginDto.Email, cancellationToken)
            ?? throw new InvalidCredentialsException();

        if (!_hashService.VerifyPasswordHash(command.UserLoginDto.Password, user.PasswordHash, user.PasswordSalt))
            throw new InvalidCredentialsException();

        TokenInfo refreshTokenInfo = _tokenService.GenerateRefreshToken();

        user.AddRefreshToken(
            refreshTokenInfo.Token,
            refreshTokenInfo.IssuedAt,
            refreshTokenInfo.ExpiresAt);
        user.RecordLogin(_dateTimeService.Now);

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        TokenInfo accessTokenInfo = _tokenService.GenerateAccessToken(user.Id.ToString(), user.FullName, user.Email);
        return new LoginResponse(accessTokenInfo, refreshTokenInfo);
    }
}
