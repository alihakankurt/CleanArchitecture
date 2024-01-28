using Core.Application.Exceptions;

namespace Application.Features.Authentication.Exceptions;

public sealed class InvalidEmailOrPasswordException : BusinessRuleException
{
    public InvalidEmailOrPasswordException() : base("The email or password is wrong")
    {
    }
}
