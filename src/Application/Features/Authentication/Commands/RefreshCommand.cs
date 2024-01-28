using Application.Models.Responses;
using Application.Persistence.Repositories;
using Core.Application.Contracts;
using Core.Application.Models;
using Core.Application.Persistence;
using Core.Application.Services;
using Domain.Entities;

namespace Application.Features.Authentication.Commands;

public sealed record RefreshCommand(string RefreshToken, string IpAddress) : IRequest<AuthenticationResponseModel>;

internal sealed class RefreshCommandHandler : IRequestHandler<RefreshCommand, AuthenticationResponseModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IDateTimeService _dateTimeService;
    private readonly AuthenticationBusinessRules _authenticationBusinessRules;

    public RefreshCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, ITokenService tokenService, IDateTimeService dateTimeService, AuthenticationBusinessRules authenticationBusinessRules)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _dateTimeService = dateTimeService;
        _authenticationBusinessRules = authenticationBusinessRules;
    }

    public async Task<AuthenticationResponseModel> HandleAsync(RefreshCommand request, CancellationToken cancellationToken = default)
    {
        User? user = await _userRepository.GetAsync(x => x.RefreshTokens.Any(x => x.Token == request.RefreshToken), cancellationToken);
        await _authenticationBusinessRules.UserMustBeExistToUseRefreshToken(user);

        user!.RefreshTokens.RemoveAll((token) => token.ExpiresAt <= _dateTimeService.Now);

        RefreshToken? refreshToken = user.RefreshTokens.LastOrDefault(x => x.Token == request.RefreshToken && x.IpAddress == request.IpAddress);
        await _authenticationBusinessRules.RefreshTokenMustBeExistToUse(refreshToken);

        await _userRepository.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        TokenModel accessTokenModel = _tokenService.GenerateAccessToken(user.Id.ToString(), user.FullName, user.Email);
        TokenModel refreshTokenModel = new(refreshToken!.Token, refreshToken.CreatedAt, refreshToken.ExpiresAt);

        return new AuthenticationResponseModel(accessTokenModel, refreshTokenModel);
    }
}
