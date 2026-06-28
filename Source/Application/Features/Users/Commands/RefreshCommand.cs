using Application.Features.Users.Exceptions;
using Application.Persistence;
using Core.Application.Persistence;
using Core.Application.Requests;
using Core.Application.Services;
using Domain.Entities;

namespace Application.Features.Users.Commands;

public sealed record RefreshCommand(string RefreshToken) : IRequest<RefreshResponse>;

public sealed record RefreshResponse(TokenInfo AccessToken);

internal sealed class RefreshCommandHandler : IRequestHandler<RefreshCommand, RefreshResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IDateTimeService _dateTimeService;

    public RefreshCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ITokenService tokenService, IDateTimeService dateTimeService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _dateTimeService = dateTimeService;
    }

    public async ValueTask<RefreshResponse> HandleAsync(RefreshCommand command, CancellationToken cancellationToken = default)
    {
        User user = await _userRepository.FindAsync((user) => user.RefreshTokens.Any((rt) => rt.Token == command.RefreshToken), cancellationToken)
            ?? throw new InvalidCredentialsException();

        DateTimeOffset now = _dateTimeService.Now;

        RefreshToken refreshToken = user.RefreshTokens.First((rt) => rt.Token == command.RefreshToken);
        if (!refreshToken.IsActive(now))
            throw new InvalidCredentialsException();

        TokenInfo accessTokenInfo = _tokenService.GenerateAccessToken(user.Id.ToString(), user.FullName, user.Email);

        user.PruneRefreshTokens(_dateTimeService.Now);

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RefreshResponse(accessTokenInfo);
    }
}
