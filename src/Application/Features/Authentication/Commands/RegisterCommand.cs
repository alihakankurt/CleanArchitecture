using Application.Models.Requests;
using Application.Models.Responses;
using Application.Persistence.Repositories;
using Core.Application.Contracts;
using Core.Application.Models;
using Core.Application.Persistence;
using Core.Application.Services;
using Domain.Entities;

namespace Application.Features.Authentication.Commands;

public sealed record RegisterCommand(RegisterRequestModel Model, string IpAddress) : IRequest<AuthenticationResponseModel>;

internal sealed class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResponseModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _hashService;
    private readonly ITokenService _tokenService;
    private readonly AuthenticationBusinessRules _authenticationBusinessRules;

    public RegisterCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IHashService hashService,
        ITokenService tokenService, AuthenticationBusinessRules authenticationBusinessRules)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _hashService = hashService;
        _tokenService = tokenService;
        _authenticationBusinessRules = authenticationBusinessRules;
    }

    public async Task<AuthenticationResponseModel> HandleAsync(RegisterCommand request, CancellationToken cancellationToken = default)
    {
        await _authenticationBusinessRules.EmailCanNotBeUsedTwiceOnRegistration(request.Model.Email, cancellationToken);
        await _authenticationBusinessRules.PasswordMustBeConfirmedOnRegistration(request.Model.Password, request.Model.ConfirmedPassword);

        _hashService.GeneratePasswordHash(request.Model.Password, out byte[] passwordHash, out byte[] passwordSalt);
        var user = new User
        {
            FirstName = request.Model.FirstName,
            LastName = request.Model.LastName,
            Email = request.Model.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
        };

        await _userRepository.CreateAsync(user);

        TokenModel refreshTokenModel = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            User = user,
            Token = refreshTokenModel.Token,
            IpAddress = request.IpAddress,
            CreatedAt = refreshTokenModel.CreatedAt,
            ExpiresAt = refreshTokenModel.ExpiresAt,
        };

        user.RefreshTokens.Add(refreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        TokenModel accessTokenModel = _tokenService.GenerateAccessToken(user.Id.ToString(), user.FullName, user.Email);
        return new AuthenticationResponseModel(accessTokenModel, refreshTokenModel);
    }
}
