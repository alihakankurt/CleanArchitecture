using Application.Features.Users.Dtos;
using Application.Features.Users.Exceptions;
using Application.Persistence;
using Core.Application.Requests;
using Domain.Entities;

namespace Application.Features.Users.Queries;

public sealed record GetCurrentUserQuery(long UserId) : IRequest<GetCurrentUserResponse>;

public sealed record GetCurrentUserResponse(UserDto User);

public sealed class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResponse>
{
    private readonly IUserRepository _userRepository;

    public GetCurrentUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async ValueTask<GetCurrentUserResponse> HandleAsync(GetCurrentUserQuery query, CancellationToken cancellationToken = default)
    {
        User user = await _userRepository.FindByIdAsync(query.UserId, cancellationToken)
            ?? throw new InvalidCredentialsException();

        return new GetCurrentUserResponse(new UserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedAt = user.CreatedAt
        });
    }
}
