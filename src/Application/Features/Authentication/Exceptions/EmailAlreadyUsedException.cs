using Core.Application.Exceptions;

namespace Application.Features.Authentication.Exceptions;

public sealed class EmailAlreadyUsedException : BusinessRuleException
{
    public EmailAlreadyUsedException() : base("The email address is already used by another account")
    {
    }
}
