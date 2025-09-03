using Application.Features.Users.Exceptions;
using Application.Persistence;
using Core.Application.Persistence;
using Core.Application.Requests;
using Core.Application.Services;
using Domain.Entities;

namespace Application.Features.Users.Commands;

public sealed record RevokeCommand(string RefreshToken) : IRequest;

internal sealed class RevokeCommandHandler : IRequestHandler<RevokeCommand>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IDateTimeService _dateTimeService;

    public RevokeCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ITokenService tokenService, IDateTimeService dateTimeService)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _dateTimeService = dateTimeService;
    }

    public async ValueTask HandleAsync(RevokeCommand command, CancellationToken cancellationToken = default)
    {
        User user = await _userRepository.FindAsync((user) => user.RefreshTokens.Any((rt) => rt.Token == command.RefreshToken), cancellationToken)
            ?? throw new InvalidCredentialsException();

        DateTimeOffset now = _dateTimeService.Now;

        RefreshToken refreshToken = user.RefreshTokens.First((refreshToken) => refreshToken.Token == command.RefreshToken);
        if (refreshToken.ExpiresAt < now)
            throw new InvalidCredentialsException();

        user.RefreshTokens.Remove(refreshToken);
        user.RemoveExpiredRefreshTokens(_dateTimeService.Now);

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
