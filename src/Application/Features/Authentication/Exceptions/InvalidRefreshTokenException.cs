using Core.Application.Exceptions;

namespace Application.Features.Authentication.Exceptions;

public sealed class InvalidRefreshTokenException : BusinessRuleException
{
    public InvalidRefreshTokenException() : base("The refresh token is invalid or expired")
    {
    }
}
