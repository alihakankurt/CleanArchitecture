using Core.Application.Contracts;
using Core.Application.Models;
using Core.Application.Services;
using Core.Application.Persistence;
using Domain.Entities;
using Application.Features.Authentication.Exceptions;
using Application.Persistence.Repositories;
using Application.Models.Requests;
using Application.Models.Responses;

namespace Application.Features.Authentication.Commands;

public sealed record LoginCommand(LoginRequestModel Model, string IpAddress) : IRequest<AuthenticationResponseModel>
{
    internal sealed class Handler : IRequestHandler<LoginCommand, AuthenticationResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        private readonly AuthenticationBusinessRules _authenticationBusinessRules;
        private readonly IDateTimeService _dateTimeService;

        public Handler(IUserRepository userRepository, IUnitOfWork unitOfWork,
            ITokenService tokenService, AuthenticationBusinessRules authenticationBusinessRules, IDateTimeService dateTimeService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _authenticationBusinessRules = authenticationBusinessRules;
            _dateTimeService = dateTimeService;
        }

        public async Task<AuthenticationResponseModel> HandleAsync(LoginCommand request, CancellationToken cancellationToken = default)
        {
            User? user = await _userRepository.GetAsync(x => x.Email == request.Model.Email, cancellationToken);
            await _authenticationBusinessRules.UserMustBeExistToUseLogIn(user);

            await _authenticationBusinessRules.PasswordMustBeValidOnLogin(request.Model.Password, user!.PasswordHash, user.PasswordSalt);

            TokenModel accessTokenModel = _tokenService.GenerateAccessToken(user.Id.ToString(), user.FullName, user.Email);
            TokenModel refreshTokenModel = _tokenService.GenerateRefreshToken();

            user.RemoveExpiredRefreshTokens(_dateTimeService.Now);
            user.RefreshTokens.Add(new RefreshToken
            {
                User = user,
                Token = refreshTokenModel.Token,
                IpAddress = request.IpAddress,
                CreatedAt = refreshTokenModel.CreatedAt,
                ExpiresAt = refreshTokenModel.ExpiresAt,
            });

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new AuthenticationResponseModel(accessTokenModel, refreshTokenModel);
        }
    }
}
