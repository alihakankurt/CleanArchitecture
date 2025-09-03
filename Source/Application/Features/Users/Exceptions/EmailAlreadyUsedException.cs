using Core.Application.BusinessRules;

namespace Application.Features.Users.Exceptions;

public sealed class EmailAlreadyUsedException : BusinessRuleException
{
    public EmailAlreadyUsedException() : base("This email address is already used by another account")
    {
    }
}
