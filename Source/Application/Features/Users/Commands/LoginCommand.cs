using Application.Features.Users.Dtos;
using Application.Features.Users.Exceptions;
using Application.Persistence;
using Core.Application.Persistence;
using Core.Application.Requests;
using Core.Application.Services;
using Domain.Entities;

namespace Application.Features.Users.Commands;

public sealed record LoginCommand(UserLoginDto UserLoginDto) : IRequest<LoginResponse>;

public sealed record LoginResponse(TokenInfo AccessToken, TokenInfo RefreshToken);

internal sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IHashService _hashService;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IHashService hashService, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _hashService = hashService;
        _tokenService = tokenService;
    }

    public async ValueTask<LoginResponse> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.FindAsync((user) => user.Email == command.UserLoginDto.Email, cancellationToken)
            ?? throw new InvalidCredentialsException();

        if (!_hashService.VerifyPasswordHash(command.UserLoginDto.Password, user.PasswordHash, user.PasswordSalt))
            throw new InvalidCredentialsException();

        TokenInfo refreshTokenInfo = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenInfo.Token,
            CreatedAt = refreshTokenInfo.CreatedAt,
            ExpiresAt = refreshTokenInfo.ExpiresAt,
        };

        user.RefreshTokens.Add(refreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        TokenInfo accessTokenInfo = _tokenService.GenerateAccessToken(user.Id.ToString(), user.FullName, user.Email);
        return new LoginResponse(accessTokenInfo, refreshTokenInfo);
    }
}
