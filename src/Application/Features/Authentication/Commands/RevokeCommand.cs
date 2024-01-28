using Core.Application.Contracts;
using Core.Application.Persistence;
using Domain.Entities;
using Application.Persistence.Repositories;

namespace Application.Features.Authentication.Commands;

public sealed record RevokeCommand(string RefreshToken) : IRequest
{
    internal sealed class Handler : IRequestHandler<RevokeCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthenticationBusinessRules _authenticationBusinessRules;

        public Handler(IUserRepository userRepository, IUnitOfWork unitOfWork, AuthenticationBusinessRules authenticationBusinessRules)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _authenticationBusinessRules = authenticationBusinessRules;
        }

        public async Task HandleAsync(RevokeCommand request, CancellationToken cancellationToken = default)
        {
            User? user = await _userRepository.GetAsync(x => x.RefreshTokens.Any(x => x.Token == request.RefreshToken), cancellationToken);
            await _authenticationBusinessRules.UserMustBeExistToUseRefreshToken(user);

            RefreshToken? refreshToken = user!.RefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken);
            await _authenticationBusinessRules.RefreshTokenMustBeExistToUse(refreshToken);
            user.RefreshTokens.Remove(refreshToken!);

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
