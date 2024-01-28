using Application.Models.Responses;
using Application.Persistence.Repositories;
using Core.Application.Contracts;
using Domain.Entities;
using Domain.Exceptions;

namespace Application.Features.Authentication.Queries;

public sealed record GetUserByIdQuery(int UserId) : IRequest<UserResponseModel>;

public sealed class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserResponseModel>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserResponseModel> HandleAsync(GetUserByIdQuery request, CancellationToken cancellationToken = default)
    {
        User user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken)
            ?? throw new UserNotFoundException(request.UserId);

        return (UserResponseModel)user;
    }
}
